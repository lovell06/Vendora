using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace Vendora.Web.Gateway;

public sealed record SessionData(
    Guid UserId, string AccessToken, Guid RefreshTokenId, string RefreshToken, string Email, string Role, DateTimeOffset ExpiresAt);

// Holds the signed-in user for one Blazor circuit. Tokens are persisted (encrypted) in the browser's
// session storage so a page reload within the same tab keeps the user signed in.
public sealed class UserSession(ProtectedSessionStorage storage)
{
    private const string Key = "vendora.session.v2";
    private bool _loaded;

    public SessionData? Current { get; private set; }
    public bool IsAuthenticated => Current is { } s && s.ExpiresAt > DateTimeOffset.UtcNow;
    public bool IsLoaded => _loaded;
    public bool IsStaff => IsAuthenticated && Current?.Role is "Admin" or "Employee";
    public bool IsCustomer => IsAuthenticated && !IsStaff;
    public string DisplayName => Current?.Email ?? "";
    public event Action? Changed;

    public async Task EnsureLoadedAsync()
    {
        if (_loaded) return;
        var wasLoaded = _loaded;
        try
        {
            var stored = await storage.GetAsync<SessionData>(Key);
            Current = stored.Success ? stored.Value : null;
            _loaded = true;
        }
        catch (Exception ex) when (ex is CryptographicException or JsonException)
        {
            _loaded = true;
            await TryDeleteAsync();
        }
        catch (InvalidOperationException)
        {
            // JS interop is not available yet (circuit not connected); try again on the next call.
        }
        catch (JSException)
        {
            _loaded = true; // storage unusable in this browser: treat as signed out instead of retrying forever
        }
        if (Current is { } s && s.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            Current = null;
            await TryDeleteAsync();
        }
        if (!wasLoaded && _loaded) Changed?.Invoke(); // headers waiting on the first load can render now
    }

    public async Task SignInAsync(LoginResponse login)
    {
        var (email, role, expires) = ReadToken(login.AccessToken);
        Current = new SessionData(login.UserId, login.AccessToken, login.RefreshToken.Id, login.RefreshToken.Value, email, role, expires);
        _loaded = true;
        try { await storage.SetAsync(Key, Current); } catch (Exception ex) when (ex is InvalidOperationException or JSException) { }
        Changed?.Invoke();
    }

    public async Task SignOutAsync()
    {
        Current = null;
        _loaded = true;
        await TryDeleteAsync();
        Changed?.Invoke();
    }

    private async Task TryDeleteAsync()
    {
        try { await storage.DeleteAsync(Key); } catch (Exception ex) when (ex is InvalidOperationException or JSException) { }
    }

    // The signature is verified by the services; the Web only reads claims for display and expiry.
    private static (string Email, string Role, DateTimeOffset Expires) ReadToken(string jwt)
    {
        try
        {
            var payload = jwt.Split('.')[1].Replace('-', '+').Replace('_', '/');
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            using var doc = JsonDocument.Parse(Convert.FromBase64String(payload));
            var root = doc.RootElement;
            var email = root.TryGetProperty("email", out var e) ? e.GetString() ?? "" : "";
            var role = root.TryGetProperty("role", out var r) ? r.GetString() ?? "" : "";
            var exp = root.TryGetProperty("exp", out var x) && x.TryGetInt64(out var seconds)
                ? DateTimeOffset.FromUnixTimeSeconds(seconds)
                : DateTimeOffset.UtcNow.AddMinutes(15);
            return (email, role, exp);
        }
        catch (Exception ex) when (ex is FormatException or JsonException or IndexOutOfRangeException)
        {
            return ("", "", DateTimeOffset.UtcNow.AddMinutes(15));
        }
    }
}
