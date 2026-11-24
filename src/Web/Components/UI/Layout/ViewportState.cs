using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;

namespace Vendora.Web.Components.UI.Layout;

// Tracks whether the browser is in the compact (mobile) width range for this circuit.
// Compact = MudBlazor Breakpoint.Xs (< 600px), the same boundary the CSS uses at 599px.
public sealed class ViewportState(IBrowserViewportService service) : IBrowserViewportObserver, IAsyncDisposable
{
    private Task? _initialization;

    public Guid Id { get; } = Guid.NewGuid();
    public ResizeOptions? ResizeOptions => null;
    public bool IsCompact { get; private set; }
    public event Action? Changed;

    // Safe to call from several components; the subscription is made once.
    public Task EnsureInitializedAsync() => _initialization ??= InitializeAsync();

    private async Task InitializeAsync()
    {
        try
        {
            IsCompact = await service.GetCurrentBreakpointAsync() == Breakpoint.Xs;
            await service.SubscribeAsync(this, fireImmediately: false);
        }
        catch (Exception ex) when (ex is JSException or InvalidOperationException)
        {
            _initialization = null; // JS is not available yet; try again on the next call.
        }
    }

    public Task NotifyBrowserViewportChangeAsync(BrowserViewportEventArgs args)
    {
        var compact = args.Breakpoint == Breakpoint.Xs;
        if (compact != IsCompact)
        {
            IsCompact = compact;
            Changed?.Invoke();
        }
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        try { await service.UnsubscribeAsync(this); }
        catch (Exception) { /* The circuit is going away. */ }
    }
}
