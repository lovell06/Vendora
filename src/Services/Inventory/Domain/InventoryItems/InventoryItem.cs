using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Inventory.Domain.InventoryItems;

public class InventoryItem
{
    public long ProductId { get; init; }
    public int OnHandQuantity { get; private set; }
    public int ReservedQuantity { get; private set; }
    public int AvailableQuantity => OnHandQuantity - ReservedQuantity;
    public bool IsAvailable => AvailableQuantity > 0;
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    private readonly List<InventoryAdjustment> _adjustments = [];
    public IReadOnlyList<InventoryAdjustment> Adjustments => _adjustments;

    private InventoryItem()
    {
    }

    public static Result<InventoryItem> Create(
        long productId,
        DateTimeOffset createdAt)
    {
        if (productId < 0)
            return Result<InventoryItem>.Failure(InventoryItemErrors.InvalidProductId);
        
        return Result<InventoryItem>.Success(new InventoryItem
        {
            ProductId = productId,
            OnHandQuantity = 0,
            ReservedQuantity = 0,
            CreatedAt = createdAt
        });
    }

    private Result IncreaseStock(int increasedQuantity, DateTimeOffset occurredAt)
    {
        if (increasedQuantity <= 0)
            return Result.Failure(InventoryItemErrors.InvalidQuantity);
        
        OnHandQuantity += increasedQuantity;
        UpdatedAt = occurredAt;

        return Result.Success();
    }

    private Result DecreaseStock(int decreasedQuantity, DateTimeOffset occurredAt)
    {
        if (decreasedQuantity <= 0)
            return Result.Failure(InventoryItemErrors.InvalidQuantity);

        if (decreasedQuantity > AvailableQuantity)
            return Result.Failure(InventoryItemErrors.InsufficientAvailableStock);

        OnHandQuantity -= decreasedQuantity;
        UpdatedAt = occurredAt;

        return Result.Success();
    }

    public Result AdjustStock(InventoryAdjustment inventoryAdjustment, DateTimeOffset occurredAt)
    {
        _adjustments.Add(inventoryAdjustment);

        var quantityChange = inventoryAdjustment.QuantityChange;
        return quantityChange switch
        {
            > 0 => IncreaseStock(quantityChange, occurredAt),
            < 0 => DecreaseStock(Math.Abs(quantityChange), occurredAt),
            _ => Result.Failure(InventoryItemErrors.QuantityChangeCannotBeZero)
        };
    }

    public Result Reserve(int quantity, DateTimeOffset occurredAt)
    {
        if (quantity <= 0)
            return Result.Failure(InventoryItemErrors.InvalidQuantity);
        
        if (quantity > AvailableQuantity)
            return Result.Failure(InventoryItemErrors.InsufficientAvailableStock);

        ReservedQuantity += quantity;
        UpdatedAt = occurredAt;

        return Result.Success();
    }

    public Result Release(int quantity, DateTimeOffset occurredAt)
    {
        if (quantity <= 0)
            return Result.Failure(InventoryItemErrors.InvalidQuantity);
        
        if (quantity > ReservedQuantity)
            return Result.Failure(InventoryItemErrors.InsufficientReservedStock);

        ReservedQuantity -= quantity;
        UpdatedAt = occurredAt;

        return Result.Success();
    }

    public Result Commit(int quantity, DateTimeOffset occurredAt)
    {
        if (quantity <= 0)
            return Result.Failure(InventoryItemErrors.InvalidQuantity);
        
        if (quantity > ReservedQuantity)
            return Result.Failure(InventoryItemErrors.InsufficientReservedStock);

        OnHandQuantity -= quantity;
        ReservedQuantity -= quantity;
        UpdatedAt = occurredAt;
        
        return Result.Success();
    }
}