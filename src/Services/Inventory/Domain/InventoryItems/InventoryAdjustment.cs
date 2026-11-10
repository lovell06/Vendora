using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Inventory.Domain.InventoryItems;

public sealed class InventoryAdjustment
{
    public Guid Id { get; init; }
    public long InventoryItemId { get; init; }
    public int QuantityChange { get; init; }
    public required string Reason { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

    private InventoryAdjustment()
    {
    }

    public static Result<InventoryAdjustment> Create(
        long inventoryItemId,
        int quantityChange,
        string reason,
        Guid createdBy,
        DateTimeOffset createdAt)
    {
        if (inventoryItemId < 0)
        {
            return Result<InventoryAdjustment>.Failure(new Error
            {
                Code = "inventory_adjustment.inventory_item_id_invalid",
                Message = "Inventory item ID invalid.",
                Type = ErrorType.Validation
            });
        }

        if (quantityChange == 0)
            return Result<InventoryAdjustment>.Failure(InventoryItemErrors.QuantityChangeCannotBeZero);

        return Result<InventoryAdjustment>.Success(new InventoryAdjustment
        {
            Id = Guid.NewGuid(),
            InventoryItemId = inventoryItemId,
            QuantityChange = quantityChange,
            Reason = reason,
            CreatedBy = createdBy,
            CreatedAt = createdAt
        });
    }
}