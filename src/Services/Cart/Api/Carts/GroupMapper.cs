using Vendora.Services.Cart.Api.Carts.AddItem;
using Vendora.Services.Cart.Api.Carts.Initialize;
using Vendora.Services.Cart.Api.Carts.RemoveItem;
using Vendora.Services.Cart.Api.Carts.SetQuantity;

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
    }
}