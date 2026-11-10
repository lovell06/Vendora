using Vendora.Services.Cart.Application.Abstractions.Clients;

namespace Vendora.Services.Cart.Application.Carts.AddItem;

public sealed class Handler(
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork,
    ICatalogServiceClient catalogServiceClient,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        var isProductExists = await catalogServiceClient.ProductExistsByIdAsync(cmd.ProductVariantId, cancellationToken);
        if (isProductExists.IsFailure)
            return Result.Failure(isProductExists.Error);

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

        var itemInCart = cart.Items.SingleOrDefault(i => i.ProductVariantId == cmd.ProductVariantId);
        
        if (itemInCart is not null)
        {
            logger.LogWarning(
                "Item {ProductId} previously added by User {UserId}.",
                cmd.ProductVariantId,
                cmd.UserId);

            itemInCart.SetQuantity(cmd.Quantity, utcNow);
        }
        else
        {
            var item = CartItem.Create(
                cartId: cart.Id,
                productVariantId: cmd.ProductVariantId,
                quantity: cmd.Quantity,
                createdAt: utcNow);

            cart.AddItem(item, utcNow);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}