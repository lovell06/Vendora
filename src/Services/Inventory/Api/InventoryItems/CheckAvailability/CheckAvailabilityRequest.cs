using Vendora.Services.Inventory.Application.InventoryItems;

namespace Vendora.Services.Inventory.Api.InventoryItems.CheckAvailability;

public sealed class CheckAvailabilityRequest
{
    public const string Pattern = "/{productVariantId}/availability";
    public long ProductVariantId { get; init; }
    public CheckInventoryAvailability.Query ToQuery()
    {
        return new CheckInventoryAvailability.Query(
            ProductVariantId);
    }
}