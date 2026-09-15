using Vendora.Services.Catalog.Api.Features.CreateProduct;

namespace Vendora.Services.Catalog.Api.Extensions;

internal static class EndpointRouteBuilderExtensions
{
    internal static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        api.MapProducts();

        return app;
    }

    private static void MapProducts(this IEndpointRouteBuilder api)
    {
        var group = api.MapGroup("/products").WithTags("Products");

        group.MapCreateProductEndpoint();
    }
}