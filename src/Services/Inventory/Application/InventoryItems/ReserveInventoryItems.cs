using Vendora.Services.Inventory.Application.Abstractions.Persistence.Exceptions;

namespace Vendora.Services.Inventory.Application.InventoryItems;

public static class ReserveInventoryItems
{
    public sealed record Command(IReadOnlyList<ReservedItemDto> Items) : ICommand;

    public readonly record struct ReservedItemDto(long ProductVariantId, int Quantity);

    public static class Errors
    {
        public static readonly Error ItemNotFound = new()
        {
            Code = "inventory_item_reserving.item_not_found",
            Message = "Inventory item not found.",
            Type = ErrorType.NotFound
        };

        public static readonly Error ItemsEmpty = new()
        {
            Code = "inventory_item_reserving.item_empty",
            Message = "Inventory items must be not empty.",
            Type = ErrorType.Validation
        };

        public static readonly Error ConcurrencyConflict = Error.Conflict(
            code: "inventory_item_reserving.concurrency_conflict",
            message: "Stock was changed by another request. Please try again.");
    }

    public sealed class Handler(
        IInventoryItemRepository inventoryItemRepository,
        IUnitOfWork unitOfWork,
        ILogger<Handler> logger,
        TimeProvider clock) : ICommandHandler<Command>
    {
        private const int MaxAttempts = 3;

        public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
        {
            if (cmd.Items.Count == 0)
                return Result.Failure(Errors.ItemsEmpty);

            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    return await ReserveOnceAsync(cmd, cancellationToken);
                }
                catch (ConcurrencyConflictException) when (attempt < MaxAttempts)
                {
                    logger.LogInformation(
                        "Reserve conflict, retrying {attempt}/{maxAttempt} times.", attempt, MaxAttempts);

                    await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.Next(1, 50) * attempt), cancellationToken);
                }
                catch (ConcurrencyConflictException)
                {
                    logger.LogWarning(
                        "Reserve conflicted {maxAttempt} times.", MaxAttempts);

                    return Result.Failure(Errors.ConcurrencyConflict);
                }
            }
        }

        private async Task<Result> ReserveOnceAsync(Command cmd, CancellationToken ct)
        {
            var utcNow = clock.GetUtcNow();

            var inventoryItems = await inventoryItemRepository.ListAsync(
                productVariantIds: [.. cmd.Items.Select(item => item.ProductVariantId)], 
                cancellationToken: ct);

            if (cmd.Items.Count != inventoryItems.Count)
                return Result.Failure(Errors.ItemNotFound);

            foreach (var item in cmd.Items)
            {
                var inventoryItem = inventoryItems.SingleOrDefault(i => i.ProductVariantId == item.ProductVariantId);
                if (inventoryItem is null)
                    return Result.Failure(Errors.ItemNotFound);

                var reservedResult = inventoryItem.Reserve(item.Quantity, utcNow);
                if (reservedResult.IsFailure)
                    return Result.Failure(reservedResult.Error);
            }

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
