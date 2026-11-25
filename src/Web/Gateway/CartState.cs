namespace Vendora.Web.Gateway;

public sealed record CartLine(
    long VariantId, long? ProductId, string ProductName, string? VariantName, int Quantity,
    decimal? UnitPrice, string? Currency)
{
    // A line whose price lookup failed cannot be selected for checkout.
    public bool HasPrice => UnitPrice is not null && Currency is not null;
    public decimal LineTotal => (UnitPrice ?? 0m) * Quantity;
    public string UnitPriceText => HasPrice ? PriceFormatter.Format(UnitPrice!.Value, Currency!) : "";
    public string PriceText => HasPrice ? PriceFormatter.Format(LineTotal, Currency!) : "";
}

// Cart of the signed-in user for this circuit; the backend cart is the source of truth.
// Selection ("which lines to buy") is client-side state: the order API takes the chosen variant ids.
public sealed class CartState(GatewayClient gateway, UserSession session)
{
    private const int QuantityDebounceMs = 400;

    private readonly HashSet<long> _selected = [];
    private readonly HashSet<long> _known = [];
    private readonly HashSet<long> _busy = [];
    private readonly Dictionary<long, CancellationTokenSource> _pending = [];
    private int _refreshVersion;

    public IReadOnlyList<CartLine> Lines { get; private set; } = [];
    public int Count => Lines.Sum(line => line.Quantity);
    public bool Loading { get; private set; }
    public string? Error { get; private set; }
    public bool PlacingOrder { get; private set; }
    public event Action? Changed;

    // Raised when a background change (quantity, remove) failed and was rolled back.
    public event Action<string>? OperationFailed;

    // ---- Selection ----

    public IReadOnlyList<CartLine> SelectedLines => Lines.Where(line => _selected.Contains(line.VariantId)).ToList();
    public int SelectedCount => SelectedLines.Count;
    public decimal SelectedTotal => SelectedLines.Sum(line => line.LineTotal);
    public bool HasMixedCurrency => SelectedLines.Select(line => line.Currency).Distinct().Count() > 1;
    public string? SelectedCurrency => SelectedLines.Select(line => line.Currency).FirstOrDefault();
    public bool AllSelected => Lines.Any(line => line.HasPrice) && Lines.Where(line => line.HasPrice).All(line => _selected.Contains(line.VariantId));
    public bool SomeSelected => _selected.Count > 0 && !AllSelected;
    public bool IsSelected(long variantId) => _selected.Contains(variantId);
    public bool IsBusy(long variantId) => _busy.Contains(variantId);
    public bool IsAnyBusy => _busy.Count > 0 || PlacingOrder;

    public void SetSelected(long variantId, bool selected)
    {
        var line = Lines.FirstOrDefault(l => l.VariantId == variantId);
        if (line is null || !line.HasPrice) return;
        if (selected) _selected.Add(variantId); else _selected.Remove(variantId);
        Changed?.Invoke();
    }

    public void SelectAll(bool selected)
    {
        _selected.Clear();
        if (selected) foreach (var line in Lines.Where(l => l.HasPrice)) _selected.Add(line.VariantId);
        Changed?.Invoke();
    }

    // ---- Loading ----

    public async Task RefreshAsync()
    {
        await session.EnsureLoadedAsync();
        if (!session.IsAuthenticated) { Clear(); return; }

        var version = ++_refreshVersion;
        Loading = true;
        Changed?.Invoke();
        try
        {
            var cart = await gateway.GetCartAsync();
            if (version != _refreshVersion) return; // A newer refresh superseded this one.
            if (cart.NotFound) { ApplyLines([]); Error = null; return; } // Cart not initialized yet = empty.
            if (!cart.Ok || cart.Value is null) { Error = cart.Error; return; }

            var items = cart.Value.Items;
            if (items.Count == 0) { ApplyLines([]); Error = null; return; }

            var lookup = await gateway.LookupVariantsAsync(items.Select(item => item.ProductVariantId));
            if (version != _refreshVersion) return;
            var byId = lookup.Ok && lookup.Value is not null
                ? lookup.Value.ProductVariants.ToDictionary(variant => variant.VariantId)
                : [];

            Error = null;
            ApplyLines(items.Select(item => byId.TryGetValue(item.ProductVariantId, out var variant)
                ? new CartLine(item.ProductVariantId, variant.ProductId, variant.ProductName, variant.VariantName,
                    item.Quantity, variant.Price, variant.Currency)
                : new CartLine(item.ProductVariantId, null, $"Item #{item.ProductVariantId}", null,
                    item.Quantity, null, null)).ToList());
        }
        finally
        {
            if (version == _refreshVersion) Loading = false;
            Changed?.Invoke();
        }
    }

    // Keeps the selection in step with the lines: new lines start selected, gone or unpriced lines drop out.
    private void ApplyLines(IReadOnlyList<CartLine> lines)
    {
        Lines = lines;
        var priced = lines.Where(l => l.HasPrice).Select(l => l.VariantId).ToHashSet();
        var ids = lines.Select(l => l.VariantId).ToHashSet();
        _selected.RemoveWhere(id => !priced.Contains(id));
        foreach (var line in lines)
            if (_known.Add(line.VariantId) && line.HasPrice) _selected.Add(line.VariantId);
        _known.IntersectWith(ids);
        _busy.RemoveWhere(id => !ids.Contains(id));
    }

    // ---- Changes ----

    public async Task<ApiResult<NoContent>> AddAsync(long variantId, int quantity)
    {
        var result = await gateway.AddToCartAsync(variantId, quantity);
        if (result.Ok) await RefreshAsync();
        return result;
    }

    public async Task<ApiResult<NoContent>> SetQuantityAsync(long variantId, int quantity)
    {
        var result = await gateway.SetCartQuantityAsync(variantId, quantity);
        if (result.Ok) await RefreshAsync();
        return result;
    }

    public async Task<ApiResult<NoContent>> RemoveAsync(long variantId)
    {
        var result = await gateway.RemoveFromCartAsync(variantId);
        if (result.Ok) await RefreshAsync();
        return result;
    }

    // Optimistic: the line and the totals change at once; the request is sent after a short pause so that
    // pressing "+" several times costs one request. A failure rolls back to the server state.
    public void ChangeQuantity(long variantId, int quantity)
    {
        var line = Lines.FirstOrDefault(l => l.VariantId == variantId);
        if (line is null || quantity < 1 || line.Quantity == quantity) return;

        Lines = Lines.Select(l => l.VariantId == variantId ? l with { Quantity = quantity } : l).ToList();
        if (_pending.Remove(variantId, out var previous)) previous.Cancel();
        var source = new CancellationTokenSource();
        _pending[variantId] = source;
        _busy.Add(variantId);
        Changed?.Invoke();
        _ = CommitQuantityAsync(variantId, quantity, source);
    }

    private async Task CommitQuantityAsync(long variantId, int quantity, CancellationTokenSource source)
    {
        try { await Task.Delay(QuantityDebounceMs, source.Token); }
        catch (OperationCanceledException) { return; }

        var result = await gateway.SetCartQuantityAsync(variantId, quantity);
        if (!_pending.TryGetValue(variantId, out var current) || current != source) return; // Changed again meanwhile.

        _pending.Remove(variantId);
        _busy.Remove(variantId);
        if (result.Ok) { Changed?.Invoke(); return; }

        OperationFailed?.Invoke(result.Error ?? Vendora.Web.Localization.Text.Get("Unable to update your cart."));
        await RefreshAsync();
    }

    // Removes lines at once in the UI. Returns the lines that were removed so the caller can offer "Undo".
    public async Task<IReadOnlyList<CartLine>> RemoveLinesAsync(IEnumerable<long> variantIds)
    {
        var ids = variantIds.ToHashSet();
        var removed = Lines.Where(l => ids.Contains(l.VariantId)).ToList();
        if (removed.Count == 0) return [];

        foreach (var id in ids) if (_pending.Remove(id, out var source)) source.Cancel();
        Lines = Lines.Where(l => !ids.Contains(l.VariantId)).ToList();
        foreach (var id in ids) { _selected.Remove(id); _known.Remove(id); _busy.Remove(id); }
        Changed?.Invoke();

        var results = await Task.WhenAll(ids.Select(id => gateway.RemoveFromCartAsync(id)));
        var failed = results.FirstOrDefault(r => !r.Ok);
        if (failed is not null)
        {
            OperationFailed?.Invoke(failed.Error ?? Vendora.Web.Localization.Text.Get("Unable to update your cart."));
            await RefreshAsync();
            return [];
        }
        return removed;
    }

    // Places an order for the selected lines. The backend removes ordered items from the cart through an
    // event, so they are dropped locally first and the cart is read again a little later.
    public async Task<ApiResult<NoContent>> CheckoutAsync()
    {
        var chosen = SelectedLines;
        if (chosen.Count == 0 || PlacingOrder)
            return ApiResult<NoContent>.Fail(Vendora.Web.Localization.Text.Get("Select at least one item to check out."), System.Net.HttpStatusCode.BadRequest);

        PlacingOrder = true;
        Changed?.Invoke();
        try
        {
            var result = await gateway.PlaceOrderAsync(chosen.Select(l => l.VariantId));
            if (result.Ok)
            {
                var ids = chosen.Select(l => l.VariantId).ToHashSet();
                Lines = Lines.Where(l => !ids.Contains(l.VariantId)).ToList();
                foreach (var id in ids) { _selected.Remove(id); _known.Remove(id); }
                _ = RefreshLaterAsync();
            }
            return result;
        }
        finally
        {
            PlacingOrder = false;
            Changed?.Invoke();
        }
    }

    private async Task RefreshLaterAsync()
    {
        await Task.Delay(2000);
        try { await RefreshAsync(); } catch (Exception) { /* The circuit may be gone. */ }
    }

    public void Clear()
    {
        foreach (var source in _pending.Values) source.Cancel();
        _pending.Clear();
        _selected.Clear();
        _known.Clear();
        _busy.Clear();
        Lines = [];
        Error = null;
        Loading = false;
        Changed?.Invoke();
    }
}
