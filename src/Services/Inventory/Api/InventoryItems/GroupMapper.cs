using Vendora.Services.Inventory.Api.InventoryItems.Create;

namespace Vendora.Services.Inventory.Api.InventoryItems;

public static class GroupMapper
{
    public static void MapInventoryItems(this RouteGroupBuilder api)
    {
        var inventoryItems = api
            .MapGroup("/inventory/items")
            .WithTags("Inventory Items");

        inventoryItems.MapCreateInventoryItemEndpoint();
    }
}