using Vendora.Services.Catalog.Api.Features.CreateCategory;
using Vendora.Services.Catalog.Api.Features.CreateProduct;

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
    }

    private static void MapCategories(this IEndpointRouteBuilder api)
    {
        var group = api.MapGroup("/categories").WithTags("Categories");

        group.MapCreateCategoryEndpoint();
    }
}