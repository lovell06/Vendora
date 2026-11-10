using Vendora.Services.Inventory.Application.InventoryItems.Get;

namespace Vendora.Services.Inventory.Api.InventoryItems.Get;

public sealed class GetInventoryItemRequest
{
    public const string Pattern = "/{productId}";

    public long ProductVariantId { get; init; }

    public Query ToQuery()
    {
        return new Query
        {
            ProductVariantId = ProductVariantId
        };
    }
}