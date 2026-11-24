using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Vendora.Web.Gateway;

namespace Vendora.Web.Authentication;

// Builds the Blazor ClaimsPrincipal from the signed-in UserSession so that
// AuthorizeView / AuthorizeRouteView can check roles. The Web never verifies the JWT
// signature; the services do that on every API call.
public sealed class SessionAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private const string AuthenticationType = "vendora";
    private readonly UserSession _session;

    public SessionAuthenticationStateProvider(UserSession session)
    {
        _session = session;
        _session.Changed += OnSessionChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await _session.EnsureLoadedAsync();

        if (!_session.IsAuthenticated || _session.Current is not { } current)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, current.UserId.ToString()),
            new Claim(ClaimTypes.Name, current.Email),
            new Claim(ClaimTypes.Email, current.Email),
            new Claim(ClaimTypes.Role, current.Role),
        };

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationType)));
    }

    private void OnSessionChanged() =>
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public void Dispose() => _session.Changed -= OnSessionChanged;
}
