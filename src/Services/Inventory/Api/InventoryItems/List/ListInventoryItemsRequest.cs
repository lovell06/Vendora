using Vendora.Services.Inventory.Application.InventoryItems;

namespace Vendora.Services.Inventory.Api.InventoryItems.List;

public sealed class ListInventoryItemsRequest
{
    public const string Pattern = "";

    public int Page { get; init; }
    public int Size { get; init; }

    public ListInventoryItems.Query ToQuery()
    {
        return new ListInventoryItems.Query(
            Page,
            Size);
    }
}