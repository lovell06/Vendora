using Vendora.Services.Catalog.Api.Categories.Create;
using Vendora.Services.Catalog.Api.Products.Create;
using Vendora.Services.Catalog.Api.Products.List;

namespace Vendora.Services.Catalog.Api.Extensions;

internal static class EndpointRouteBuilderExtensions
{
    internal static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        api.MapProducts();

        api.MapCategories();

        return app;
    }

    private static void MapProducts(this IEndpointRouteBuilder api)
    {
        var group = api.MapGroup("/products").WithTags("Products");

        group.MapCreateProductEndpoint();
        group.MapListProductEndpoint();
    }

    private static void MapCategories(this IEndpointRouteBuilder api)
    {
        var group = api.MapGroup("/categories").WithTags("Categories");

        group.MapCreateCategoryEndpoint();
    }
}