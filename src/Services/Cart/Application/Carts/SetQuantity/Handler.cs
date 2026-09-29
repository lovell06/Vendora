using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Cart.Application.Abstractions.Persistence;
using Vendora.Services.Cart.Domain.Carts;

namespace Vendora.Services.Cart.Application.Carts.SetQuantity;

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
                "Item quantity setting reject because user {UserId} has not initialized a cart.",
                cmd.UserId);
            
            return Result.Failure(new Error
            {
                Code = "cart_set_item_quantity.cart_not_found",
                Message = "The user has not initialized a cart.",
                Type = ErrorType.NotFound
            });
        }

        var result = cart.SetItemQuantity(cmd.ProductId, cmd.NewQuantity, utcNow);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}