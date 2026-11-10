namespace Vendora.Services.Inventory.Application.InventoryItems.ReleaseStock;

public sealed class Handler(
    IInventoryItemRepository inventoryItemRepository,
    IUnitOfWork unitOfWork,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        var inventoryItems = await inventoryItemRepository.ListAsync(
            productVariantIds: [.. cmd.Items.Select(i => i.ProductVariantId)],
            cancellationToken: cancellationToken);

        var dict = cmd.Items.ToDictionary(i => i.ProductVariantId);

        foreach (var inventoryItem in inventoryItems)
        {
            var releaseStockResult = inventoryItem.Release(
                quantity: dict[inventoryItem.ProductVariantId].Quantity, 
                occurredAt: utcNow);

            if (releaseStockResult.IsFailure)
                return Result.Failure(releaseStockResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}