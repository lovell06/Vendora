namespace Vendora.Services.Cart.Application.Carts.RemoveItem;

public sealed class Handler(
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        var cart = await cartRepository.GetWithItemsAsync(cmd.UserId, cancellationToken);

        if (cart is null)
        {
            logger.LogWarning(
                "Item removing reject because user {UserId} has not initialized a cart.",
                cmd.UserId);
            
            return Result.Failure(new Error
            {
                Code = "cart_remove_item.not_found",
                Message = "The user has not initialized a cart.",
                Type = ErrorType.NotFound
            });
        }

        var removedItem = cart.Items.SingleOrDefault(item => item.ProductId == cmd.ProducId);
        
        if (removedItem is null)
        {
            logger.LogInformation(
                "Item {ProductId} has not added to cart.",
                cmd.ProducId);
            
            return Result.Success();
        }

        cart.RemoveItem(removedItem, utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}