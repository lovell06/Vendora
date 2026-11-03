namespace Vendora.Services.Ordering.Application.Orders.Deliver;

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
                Code = "delivering_order.order_not_found",
                Message = "Order not found",
                Type = ErrorType.NotFound
            });

        var deliverOrderResult = order.Deliver(utcNow);

        if (deliverOrderResult.IsFailure)
            return Result.Failure(deliverOrderResult.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}