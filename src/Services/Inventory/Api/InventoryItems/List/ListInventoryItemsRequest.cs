using Vendora.Services.Inventory.Application.InventoryItems.List;

namespace Vendora.Services.Inventory.Api.InventoryItems.List;

public sealed class ListInventoryItemsRequest
{
    public const string Pattern = "";

    public int Page { get; init; }
    public int Size { get; init; }

    public Query ToQuery()
    {
        return new Query()
        {
            Page = Page,
            Size = Size
        };
    }
}