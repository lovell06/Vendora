using Vendora.Services.Cart.Api.Carts.GetSelectedItems;

namespace Vendora.Services.Cart.Api.Carts;

public static class GroupMapper
{
    public static void MapCarts(this RouteGroupBuilder api)
    {
        var carts = api.MapGroup("/carts").WithTags("Carts");

        carts.MapAddItemEndpoint();
        carts.MapRemoveItemEndpoint();
        carts.MapSetQuantityEndpoint();
        carts.MapListItemsEndpoint();
        carts.MapGetSelectedItemsEndpoint();
    }
}