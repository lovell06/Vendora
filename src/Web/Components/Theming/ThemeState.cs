using Microsoft.JSInterop;

namespace Vendora.Web.Components.Theming;

/// <summary>Browser theme preference shared by MudBlazor providers and theme controls in one circuit.</summary>
public sealed class ThemeState(IJSRuntime js) : IAsyncDisposable
{
    private DotNetObjectReference<ThemeState>? _reference;
    private int? _subscription;
    public string Preference { get; private set; } = "system";
    public bool IsDark { get; private set; }
    public event Action? Changed;

    public async Task InitializeAsync()
    {
        if (_reference is not null) return;
        _reference = DotNetObjectReference.Create(this);
        _subscription = await js.InvokeAsync<int>("vendoraTheme.subscribe", _reference);
        Update(await js.InvokeAsync<ThemeSnapshot>("vendoraTheme.get"));
    }

    public async Task SetPreferenceAsync(string preference)
    {
        if (preference is not ("system" or "light" or "dark"))
            throw new ArgumentOutOfRangeException(nameof(preference));
        Update(await js.InvokeAsync<ThemeSnapshot>("vendoraTheme.set", preference));
    }

    [JSInvokable]
    public void ThemeChanged(ThemeSnapshot snapshot) => Update(snapshot);

    private void Update(ThemeSnapshot snapshot)
    {
        if (Preference == snapshot.Preference && IsDark == snapshot.IsDark) return;
        Preference = snapshot.Preference;
        IsDark = snapshot.IsDark;
        Changed?.Invoke();
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_subscription.HasValue)
                await js.InvokeVoidAsync("vendoraTheme.unsubscribe", _subscription.Value);
        }
        catch (JSDisconnectedException) { }
        finally { _reference?.Dispose(); }
    }

    public sealed record ThemeSnapshot(string Preference, bool IsDark);
}
