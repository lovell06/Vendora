using Vendora.Services.Catalog.Api.Categories;
using Vendora.Services.Catalog.Api.Products;

namespace Vendora.Services.Catalog.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        api.MapProducts();
        api.MapCategories();

        return app;
    }
}