namespace Vendora.Services.Cart.Application.Carts.AddItem;

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
                "Item adding reject because user {UserId} has not initialized a cart.",
                cmd.UserId);
            
            return Result.Failure(new Error
            {
                Code = "cart_add_item.not_found",
                Message = "The user has not initialized a cart.",
                Type = ErrorType.NotFound
            });
        }

        var itemInCart = cart.Items.SingleOrDefault(i => i.ProductId == cmd.ProductId);
        
        if (itemInCart is not null)
        {
            logger.LogWarning(
                "Item {ProductId} previously added by User {UserId}.",
                cmd.ProductId,
                cmd.UserId);

            itemInCart.SetQuantity(cmd.Quantity, utcNow);
        }
        else
        {
            var item = CartItem.Create(
                cartId: cart.Id,
                productId: cmd.ProductId,
                quantity: cmd.Quantity,
                createdAt: utcNow);

            cart.AddItem(item, utcNow);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}