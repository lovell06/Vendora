using Vendora.Services.Identity.Api.Features.Auth.Register;
using Vendora.Services.Identity.Api.Features.Auth.VerifyEmail;

namespace Vendora.Services.Identity.Api.Extensions;

public static class EndpointRouteBuilderExtension
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api");

        api.MapAuth();

        return app;
    }

    private static void MapAuth(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/auth").WithTags("Auth");
        auth.MapRegisterEndpoint();
        auth.MapVerifyEmailEndpoint();
    }
}