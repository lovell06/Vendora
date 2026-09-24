using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Inventory.Application.Abstractions.Persistence;
using Vendora.Services.Inventory.Domain.InventoryItems;

namespace Vendora.Services.Inventory.Application.InventoryItems.Adjust;

public sealed class Handler(
    IInventoryItemRepository inventoryItemRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        var inventoryItem = await inventoryItemRepository.GetAsync(cmd.ProductId, cancellationToken);

        if (inventoryItem is null)
        {
            logger.LogWarning("Inventory Items Adjustment rejected because: Product is not existed.");

            return Result.Failure(new Error
            {
                Code = "inventory_items_adjustment.not_found",
                Message = "Product is not found.",
                Type = ErrorType.NotFound
            });
        }

        var createdAdjustmentResult = InventoryAdjustment.Create(
            productId: cmd.ProductId,
            quantityChange: cmd.QuantityChange,
            reason: cmd.Reason,
            createdBy: cmd.CreatedBy,
            createdAt: utcNow);
        
        if (createdAdjustmentResult.IsFailure)
        {
            logger.LogWarning("Inventory Items Adjustment rejected because: Product is not existed.");

            return Result.Failure(createdAdjustmentResult.Error);
        }

        var adjustmentResult = inventoryItem.AdjustStock(createdAdjustmentResult.Value, utcNow);
        if (adjustmentResult.IsFailure)
        {
            logger.LogWarning(
                "Inventory Items Adjustment rejected because: {error}",
                adjustmentResult.Error.Message);

            return Result.Failure(adjustmentResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}