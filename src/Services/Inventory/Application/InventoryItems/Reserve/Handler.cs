namespace Vendora.Services.Inventory.Application.InventoryItems.Reserve;

public sealed class Handler(
    IInventoryItemRepository inventoryItemRepository,
    IUnitOfWork unitOfWork,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        if (cmd.Items.Count == 0)
            return Result.Failure(Errors.ItemsEmpty);
        
        var utcNow = clock.GetUtcNow();

        var inventoryItems = await inventoryItemRepository.ListAsync(
            productIds: [.. cmd.Items.Select(item => item.ProductId)], 
            cancellationToken: cancellationToken);

        if (cmd.Items.Count != inventoryItems.Count)
            return Result.Failure(Errors.ItemNotFound);

        foreach (var item in cmd.Items)
        {
            var inventoryItem = inventoryItems.SingleOrDefault(i => i.ProductId == item.ProductId);
            if (inventoryItem is null)
                return Result.Failure(Errors.ItemNotFound);

            var reservedResult = inventoryItem.Reserve(item.Quantity, utcNow);
            if (reservedResult.IsFailure)
                return Result.Failure(reservedResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}