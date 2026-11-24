namespace Vendora.Services.Cart.Api.Carts;

public static class GroupMapper
{
    public static void MapCartEndpoints(this RouteGroupBuilder api)
    {
        var carts = api.MapGroup("/carts").WithTags("Carts");

        carts.MapAddCartItemEndpoint();
        carts.MapRemoveCartItemEndpoint();
        carts.MapSetCartItemQuantityEndpoint();
        carts.MapListCartItemsEndpoint();
        carts.MapGetSelectedCartItemsEndpoint();
    }
}