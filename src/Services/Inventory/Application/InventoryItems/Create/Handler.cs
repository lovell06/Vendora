using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Inventory.Application.Abstractions.Persistence;
using Vendora.Services.Inventory.Domain.InventoryItems;

namespace Vendora.Services.Inventory.Application.InventoryItems.Create;

public sealed class Handler(
    IInventoryItemRepository inventoryItemRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        if (await inventoryItemRepository.ExistsByProductId(cmd.ProductId, cancellationToken))
        {
            logger.LogWarning("Inventory Item Creation rejected because product not existed.");

            return Result.Failure(new Error
            {
                Code = "inventory_item_creation.existed",
                Message = "Product is existed.",
                Type = ErrorType.Conflict
            });
        }

        var result = InventoryItem.Create(
            cmd.ProductId,
            cmd.OnHandQuantity,
            utcNow);

        if (result.IsFailure)
        {
            logger.LogWarning(
                "Inventory Item Creation rejected because: {Error}",
                result.Error.Message);
            
            return Result.Failure(result.Error);
        }

        inventoryItemRepository.Add(result.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}