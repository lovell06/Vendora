namespace Vendora.Services.Inventory.Application.InventoryItems.CheckAvailability;

public sealed class Response
{
    public long ProductVariantId { get; init; }
    public bool IsAvailable { get; init; }
    public int AvailableQuantity { get; init; }
}