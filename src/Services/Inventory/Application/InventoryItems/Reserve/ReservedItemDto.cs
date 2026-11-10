namespace Vendora.Services.Inventory.Application.InventoryItems.Reserve;

public struct ReservedItemDto
{
    public long ProductVariantId { get; init; }
    public int Quantity { get; init; }
}