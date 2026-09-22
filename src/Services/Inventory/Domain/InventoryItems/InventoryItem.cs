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

    private InventoryItem()
    {
    }

    public static Result<InventoryItem> Create(
        long productId,
        int onHandQuantity,
        DateTimeOffset createdAt)
    {
        if (productId < 0)
            return Result<InventoryItem>.Failure(InventoryItemErrors.InvalidProductId);
        if (onHandQuantity <= 0)
            return Result<InventoryItem>.Failure(InventoryItemErrors.InvalidInitialQuantity);
        
        return Result<InventoryItem>.Success(new InventoryItem
        {
            ProductId = productId,
            OnHandQuantity = onHandQuantity,
            CreatedAt = createdAt
        });
    }

    public Result IncreaseStock(int quantity, DateTimeOffset occurredAt)
    {
        if (quantity <= 0)
            return Result.Failure(InventoryItemErrors.InvalidQuantity);
        
        OnHandQuantity += quantity;
        UpdatedAt = occurredAt;

        return Result.Success();
    }

    public Result DecreaseStock(int quantity, DateTimeOffset occurredAt)
    {
        if (quantity <= 0)
            return Result.Failure(InventoryItemErrors.InvalidQuantity);

        if (quantity > AvailableQuantity)
            return Result.Failure(InventoryItemErrors.InsufficientAvailableStock);

        OnHandQuantity -= quantity;
        UpdatedAt = occurredAt;

        return Result.Success();
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