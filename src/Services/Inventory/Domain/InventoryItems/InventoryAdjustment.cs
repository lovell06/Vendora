using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Inventory.Domain.InventoryItems;

public sealed class InventoryAdjustment
{
    public Guid Id { get; init; }
    public long ProductId { get; init; }
    public int QuantityChange { get; init; }
    public required string Reason { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

    private InventoryAdjustment()
    {
    }

    public static Result<InventoryAdjustment> Create(
        long productId,
        int quantityChange,
        string reason,
        Guid createdBy,
        DateTimeOffset createdAt)
    {
        if (productId < 0)
        {
            return Result<InventoryAdjustment>.Failure(new Error
            {
                Code = "inventory_adjustment.product_id_invalid",
                Message = "ProductId invalid.",
                Type = ErrorType.Validation
            });
        }

        if (quantityChange == 0)
            return Result<InventoryAdjustment>.Failure(InventoryItemErrors.QuantityChangeCannotBeZero);

        return Result<InventoryAdjustment>.Success(new InventoryAdjustment
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            QuantityChange = quantityChange,
            Reason = reason,
            CreatedBy = createdBy,
            CreatedAt = createdAt
        });
    }
}