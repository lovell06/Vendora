namespace Vendora.Services.Ordering.Application.Orders.Prepare;

public sealed class Handler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        var order = await orderRepository.GetAsync(cmd.OrderId, cancellationToken);

        if (order is null)
            return Result.Failure(new Error
            {
                Code = "preparing_order.order_not_found",
                Message = "Order not found",
                Type = ErrorType.NotFound
            });

        var prepareOrderResult = order.Prepare(utcNow);

        if (prepareOrderResult.IsFailure)
            return Result.Failure(prepareOrderResult.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}