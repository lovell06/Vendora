namespace Vendora.Services.Cart.Application.Carts;

public static class RemoveOrderedItems
{
    public sealed record Command(Guid UserId, IReadOnlyList<long> Items) : ICommand;
    
    public sealed class Handler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork,
        TimeProvider clock) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
        {
            var utcNow = clock.GetUtcNow();

            var cart = await cartRepository.GetWithItemsAsync(cmd.UserId, cancellationToken);
            if (cart is null)
                return Result.Failure(Errors.UserNotFound);

            cart.RemoveItems(cmd.Items, utcNow);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }

    public static class Errors
    {
        public static readonly Error UserNotFound = new()
        {
            Code = "removing_ordered_items.user_not_found",
            Message = "The user has not initialized a cart.",
            Type = ErrorType.NotFound
        };
    }
}