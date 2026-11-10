namespace Vendora.Services.Inventory.Application.InventoryItems.CheckAvailability;

public sealed class Query : IQuery<Response>
{
    public long ProductVariantId { get; init; }
}