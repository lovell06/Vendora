namespace Vendora.Services.Cart.Application.Carts.Initialize;

using CartAggregate = Domain.Carts.Cart;

public sealed class Handler(
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        if (await cartRepository.ExistsByUserId(cmd.UserId, cancellationToken))
        {
            logger.LogWarning(
                "Cart initialization rejected because cart for user: {CmdUserId} already.",
                cmd.UserId);

            return Result.Failure(new Error
            {
                Code = "cart_initialize.exists",
                Message = "Cart already exists.",
                Type = ErrorType.Conflict
            });
        }

        var cart = CartAggregate.Create(cmd.UserId, utcNow);

        cartRepository.Add(cart);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}