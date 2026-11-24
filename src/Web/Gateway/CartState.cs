namespace Vendora.Web.Gateway;

public sealed record CartLine(
    long VariantId, long? ProductId, string ProductName, string? VariantName, int Quantity,
    decimal? UnitPrice, string? Currency)
{
    public string PriceText => UnitPrice is { } price && Currency is not null
        ? PriceFormatter.Format(price * Quantity, Currency)
        : "";
}

// Cart of the signed-in user for this circuit; the backend cart is the source of truth.
public sealed class CartState(GatewayClient gateway, UserSession session)
{
    public IReadOnlyList<CartLine> Lines { get; private set; } = [];
    public int Count => Lines.Sum(line => line.Quantity);
    public bool Loading { get; private set; }
    public string? Error { get; private set; }
    public event Action? Changed;

    public async Task RefreshAsync()
    {
        await session.EnsureLoadedAsync();
        if (!session.IsAuthenticated) { Clear(); return; }

        Loading = true;
        Changed?.Invoke();
        try
        {
            var cart = await gateway.GetCartAsync();
            if (cart.NotFound) { Lines = []; Error = null; return; } // Cart not initialized yet = empty.
            if (!cart.Ok || cart.Value is null) { Error = cart.Error; return; }

            Error = null;
            var items = cart.Value.Items;
            if (items.Count == 0) { Lines = []; return; }

            var lookup = await gateway.LookupVariantsAsync(items.Select(item => item.ProductVariantId));
            var byId = lookup.Ok && lookup.Value is not null
                ? lookup.Value.ProductVariants.ToDictionary(variant => variant.VariantId)
                : [];

            Lines = items.Select(item => byId.TryGetValue(item.ProductVariantId, out var variant)
                ? new CartLine(item.ProductVariantId, variant.ProductId, variant.ProductName, variant.VariantName,
                    item.Quantity, variant.Price, variant.Currency)
                : new CartLine(item.ProductVariantId, null, $"Item #{item.ProductVariantId}", null,
                    item.Quantity, null, null)).ToList();
        }
        finally
        {
            Loading = false;
            Changed?.Invoke();
        }
    }

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

    public void Clear()
    {
        Lines = [];
        Error = null;
        Loading = false;
        Changed?.Invoke();
    }
}
