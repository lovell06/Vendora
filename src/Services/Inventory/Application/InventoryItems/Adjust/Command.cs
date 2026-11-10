namespace Vendora.Services.Inventory.Application.InventoryItems.Adjust;

public sealed class Command : ICommand
{
    public long InventoryItemId { get; init; }
    public int QuantityChange { get; init; }
    public required string Reason { get; init; }
    public Guid CreatedBy { get; init; }
}