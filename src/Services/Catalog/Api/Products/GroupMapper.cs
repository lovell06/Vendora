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