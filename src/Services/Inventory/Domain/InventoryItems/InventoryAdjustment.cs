namespace Vendora.Services.Inventory.Domain.InventoryItems;

public sealed class InventoryAdjustment
{
    public Guid Id { get; init; }
    public long ProductId { get; init; }
    public int QuantityChange { get; init; }
    public required string Reason { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}