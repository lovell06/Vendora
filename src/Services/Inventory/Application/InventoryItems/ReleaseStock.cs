using Vendora.Services.Inventory.Application.Abstractions.Persistence.Exceptions;

namespace Vendora.Services.Inventory.Application.InventoryItems;

public static class ReleaseStock
{
    public sealed record Command(IReadOnlyList<ItemDto> Items) : ICommand;

    public sealed record ItemDto(long ProductVariantId, int Quantity);

    public sealed class Handler(
        IInventoryItemRepository inventoryItemRepository,
        IUnitOfWork unitOfWork,
        ILogger<Handler> logger,
        TimeProvider clock) : ICommandHandler<Command>
    {
        private const uint MaxAttempts = 3;

        public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
        {
            for (var attempt = 1;; attempt++)
            {
                try
                {
                    return await ReleaseOnceAsync(cmd, cancellationToken);
                }
                catch (ConcurrencyConflictException) when (attempt < MaxAttempts)
                {
                    logger.LogInformation(
                        "Release stock conflict, retrying {attempt}/{maxAttempt} times.", attempt, MaxAttempts);

                    await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.Next(1, 50) * attempt), cancellationToken);
                }
            }
        }

        private async Task<Result> ReleaseOnceAsync(Command cmd, CancellationToken ct)
        {
            var utcNow = clock.GetUtcNow();

            var inventoryItems = await inventoryItemRepository.ListAsync(
                productVariantIds: [.. cmd.Items.Select(i => i.ProductVariantId)],
                cancellationToken: ct);

            var dict = cmd.Items.ToDictionary(i => i.ProductVariantId);

            foreach (var inventoryItem in inventoryItems)
            {
                var releaseStockResult = inventoryItem.Release(
                    quantity: dict[inventoryItem.ProductVariantId].Quantity, 
                    occurredAt: utcNow);

                if (releaseStockResult.IsFailure)
                    return Result.Failure(releaseStockResult.Error);
            }

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
