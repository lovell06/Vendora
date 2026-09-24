namespace Vendora.Services.Inventory.Application.InventoryItems.CheckAvailability;

public sealed class Response
{
    public long ProductId { get; init; }
    public bool Available { get; init; }
    public int AvailableQuantity { get; init; }
}