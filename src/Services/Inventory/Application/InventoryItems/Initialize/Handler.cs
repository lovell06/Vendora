namespace Vendora.Services.Inventory.Application.InventoryItems.Initialize;

public sealed class Handler(
    IInventoryItemRepository inventoryItemRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        if (await inventoryItemRepository.ExistsByProductVariantId(cmd.ProductVariantId, cancellationToken))
        {
            logger.LogWarning("Inventory Item Creation rejected because product variant existed.");

            return Result.Failure(new Error
            {
                Code = "inventory_item_creation.existed",
                Message = "Product variant is existed.",
                Type = ErrorType.Conflict
            });
        }

        var result = InventoryItem.Create(
            productId: cmd.ProductId,
            productVariantId: cmd.ProductVariantId,
            sku: cmd.Sku,
            createdAt: utcNow);

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