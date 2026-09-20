using Vendora.Services.Identity.Api.Auth.Login;
using Vendora.Services.Identity.Api.Auth.Logout;
using Vendora.Services.Identity.Api.Auth.Register;
using Vendora.Services.Identity.Api.Auth.VerifyEmail;

namespace Vendora.Services.Identity.Api.Auth;

public static class GroupMapper
{
    public static void MapAuth(this RouteGroupBuilder api)
    {
        var auth = api.MapGroup("/auth").WithTags("Auth");
        auth.MapRegisterEndpoint();
        auth.MapVerifyEmailEndpoint();
        auth.MapLoginEndpoint();
        auth.MapLogoutEndpoint();
    }
}