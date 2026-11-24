using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Vendora.Web.Components.UI.Layout;
using Vendora.Web.Gateway;

namespace Vendora.Web.Features.Cart;

// Decides how the cart is shown: a floating panel on wide screens, the /cart page on compact ones.
public sealed class CartUiState : IDisposable
{
    private readonly IDialogService _dialogs;
    private readonly NavigationManager _navigation;
    private readonly ViewportState _viewport;
    private readonly UserSession _session;
    private IDialogReference? _dialog;

    public CartUiState(IDialogService dialogs, NavigationManager navigation, ViewportState viewport, UserSession session)
    {
        _dialogs = dialogs;
        _navigation = navigation;
        _viewport = viewport;
        _session = session;
        _viewport.Changed += OnViewportChanged;
    }

    // True when the user reached /cart from inside the app, so "back" can pop history instead of jumping to /.
    public bool CameFromInApp { get; set; }

    // The element to give focus back to when the panel closes.
    public ElementReference? Trigger { get; set; }

    public bool IsDrawerOpen => _dialog is not null;

    public async Task OpenAsync()
    {
        await _session.EnsureLoadedAsync();
        if (!_session.IsAuthenticated)
        {
            _navigation.NavigateTo($"/login?returnUrl={Uri.EscapeDataString("/cart")}");
            return;
        }

        await _viewport.EnsureInitializedAsync();
        if (_viewport.IsCompact)
        {
            CameFromInApp = true;
            _navigation.NavigateTo("/cart");
            return;
        }

        if (_dialog is not null) return;

        var options = new DialogOptions
        {
            Position = DialogPosition.CenterRight, MaxWidth = MaxWidth.False, NoHeader = true,
            BackdropClick = false, CloseOnEscapeKey = false, CloseOnNavigation = true, // Esc and backdrop clicks are handled by CartDrawer so the exit animation plays.
            BackgroundClass = "overlay-blur-backdrop"
        };
        // The pop-out origin is set in the browser (cart.js) at click time, without a round trip.
        var reference = await _dialogs.ShowAsync<CartDrawer>("Your cart", options);
        _dialog = reference;
        _ = WatchAsync(reference);
    }

    public void CloseDrawer() => _dialog?.Close();

    private async Task WatchAsync(IDialogReference reference)
    {
        await reference.Result;
        if (ReferenceEquals(_dialog, reference)) _dialog = null;
        if (Trigger is { } trigger)
        {
            try { await trigger.FocusAsync(); }
            catch (Exception) { /* The trigger left the page (for example after navigation). */ }
        }
    }

    // The window became narrow while the panel was open: continue on the full page instead.
    private void OnViewportChanged()
    {
        if (!_viewport.IsCompact || _dialog is null) return;
        _dialog.Close();
        CameFromInApp = true;
        _navigation.NavigateTo("/cart");
    }

    public void Dispose() => _viewport.Changed -= OnViewportChanged;
}
