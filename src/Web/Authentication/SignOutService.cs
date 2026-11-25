using Microsoft.AspNetCore.Components;
using Vendora.Web.Gateway;

namespace Vendora.Web.Authentication;

// The single place that ends a session: revoke the refresh token (failure is ignored), clear the cart, drop the local session.
public sealed class SignOutService(
    GatewayClient gateway, UserSession session, CartState cart, NavigationManager navigation)
{
    public async Task SignOutAsync(string redirectTo = "/")
    {
        try { await gateway.LogoutAsync(); } catch (Exception) { /* sign out locally regardless */ }
        cart.Clear();
        await session.SignOutAsync();
        navigation.NavigateTo(redirectTo, replace: true);
    }
}
