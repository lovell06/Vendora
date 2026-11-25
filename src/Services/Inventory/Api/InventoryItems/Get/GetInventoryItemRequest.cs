using Vendora.Services.Inventory.Application.InventoryItems;

namespace Vendora.Services.Inventory.Api.InventoryItems.Get;

public sealed class GetInventoryItemRequest
{
    public const string Pattern = "/{productVariantId:long}";

    public long ProductVariantId { get; init; }

    public GetInventoryItem.Query ToQuery()
    {
        return new GetInventoryItem.Query(
            ProductVariantId);
    }
}