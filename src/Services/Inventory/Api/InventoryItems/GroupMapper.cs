using Vendora.Services.Inventory.Api.InventoryItems.Get;
using Vendora.Services.Inventory.Api.InventoryItems.Initialize;
using Vendora.Services.Inventory.Api.InventoryItems.List;

namespace Vendora.Services.Inventory.Api.InventoryItems;

public static class GroupMapper
{
    public static void MapInventoryItems(this RouteGroupBuilder api)
    {
        var inventoryItems = api
            .MapGroup("/inventory-items")
            .WithTags("Inventory Items");

        inventoryItems.MapCreateInventoryItemEndpoint();
        inventoryItems.MapGetInventoryItemEndpoint();
        inventoryItems.MapListInventoryItemsEndpoint();
    }
}