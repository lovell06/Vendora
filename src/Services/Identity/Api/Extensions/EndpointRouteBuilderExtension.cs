using Vendora.Services.Identity.Api.Auth.Login;
using Vendora.Services.Identity.Api.Auth.Logout;
using Vendora.Services.Identity.Api.Auth.Register;
using Vendora.Services.Identity.Api.Auth.VerifyEmail;

namespace Vendora.Services.Identity.Api.Extensions;

public static class EndpointRouteBuilderExtension
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        api.MapAuth();

        return app;
    }

    private static void MapAuth(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/auth").WithTags("Auth");
        auth.MapRegisterEndpoint();
        auth.MapVerifyEmailEndpoint();
        auth.MapLoginEndpoint();
        auth.MapLogoutEndpoint();
    }
}