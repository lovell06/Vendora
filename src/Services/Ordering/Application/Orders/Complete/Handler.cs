namespace Vendora.Services.Ordering.Application.Orders.Complete;

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
                Code = "completing_order.order_not_found",
                Message = "Order not found",
                Type = ErrorType.NotFound
            });

        var completeOrderResult = order.Complete(utcNow);

        if (completeOrderResult.IsFailure)
            return Result.Failure(completeOrderResult.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}