using Vendora.Services.Catalog.Api.Products.Create;
using Vendora.Services.Catalog.Api.Products.Delete;
using Vendora.Services.Catalog.Api.Products.Discontinue;
using Vendora.Services.Catalog.Api.Products.Get;
using Vendora.Services.Catalog.Api.Products.List;
using Vendora.Services.Catalog.Api.Products.Reactivate;
using Vendora.Services.Catalog.Api.Products.Restore;
using Vendora.Services.Catalog.Api.Products.Update;

namespace Vendora.Services.Catalog.Api.Products;

public static class GroupMapper
{
    public static void MapProducts(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/products").WithTags("Products");

        group.MapCreateProductEndpoint();
        group.MapListProductEndpoint();
        group.MapGetProductEndpoint();
        group.MapUpdateProductEndpoint();
        group.MapDeleteProductEndpoint();
        group.MapRestoreProductEndpoint();
        group.MapDiscontinueProductEndpoint();
        group.MapReactivateProductEndpoint();
    }
}