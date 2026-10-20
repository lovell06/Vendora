namespace Vendora.Services.Inventory.Application.InventoryItems.Reserve;

public struct ReservedItemDto
{
    public long ProductId { get; init; }
    public int Quantity { get; init; }
}