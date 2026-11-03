using Vendora.Services.Identity.Api.Users;

namespace Vendora.Services.Identity.Api.Extensions;

public static class EndpointRouteBuilderExtension
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        api.MapAuth();
        api.MapUserEndpoints();

        return app;
    }
}