using Vendora.Services.Catalog.Api.Categories.Create;
using Vendora.Services.Catalog.Api.Products.Create;
using Vendora.Services.Catalog.Api.Products.Delete;
using Vendora.Services.Catalog.Api.Products.Discontinue;
using Vendora.Services.Catalog.Api.Products.Get;
using Vendora.Services.Catalog.Api.Products.List;
using Vendora.Services.Catalog.Api.Products.Restore;
using Vendora.Services.Catalog.Api.Products.Update;

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
        group.MapGetProductEndpoint();
        group.MapUpdateProductEndpoint();
        group.MapDeleteProductEndpoint();
        group.MapRestoreProductEndpoint();
        group.MapDiscontinueProductEndpoint();
    }

    private static void MapCategories(this IEndpointRouteBuilder api)
    {
        var group = api.MapGroup("/categories").WithTags("Categories");

        group.MapCreateCategoryEndpoint();
    }
}