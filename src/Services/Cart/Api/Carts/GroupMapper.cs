namespace Vendora.Services.Cart.Api.Carts;

public static class GroupMapper
{
    public static void MapCarts(this RouteGroupBuilder api)
    {
        var carts = api.MapGroup("/carts").WithTags("Carts");

        carts.MapInitializeCartEndpoint();
        carts.MapAddItemEndpoint();
        carts.MapRemoveItemEndpoint();
        carts.MapSetQuantityEndpoint();
        carts.MapListItemsEndpoint();
    }
}