namespace Vendora.Services.Cart.Application.Carts.RemoveOrderedItems;

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