using Vendora.Services.Inventory.Application.InventoryItems.CheckAvailability;

namespace Vendora.Services.Inventory.Api.InventoryItems.CheckAvailability;

public sealed class CheckAvailabilityRequest
{
    public const string Pattern = "/{productId}/availability";
    public long ProductId { get; init; }
    public Query ToQuery()
    {
        return new Query
        {
            ProductId = ProductId
        };
    }
}