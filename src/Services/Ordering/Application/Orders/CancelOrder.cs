namespace Vendora.Services.Ordering.Application.Orders;

public static class CancelOrder
{
    public sealed record Command(Guid UserId, long OrderId) : ICommand;

    public static class Errors
    {
        public static readonly Error OrderNotFound = new()
        {
            Code = "cancelling_order.order_not_found",
            Message = "Order not found.",
            Type = ErrorType.NotFound
        };
    }

    public sealed class Handler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IOutboxMessageRepository outboxMessageRepository,
        TimeProvider clock) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
        {
            var utcNow = clock.GetUtcNow();

            var order = await orderRepository.GetWithItemAsync(cmd.OrderId, cancellationToken);

            if (order is null || order.UserId != cmd.UserId)
                return Result.Failure(Errors.OrderNotFound);

            var cancelOrderResult = order.Cancel(utcNow);

            if (cancelOrderResult.IsFailure)
                return Result.Failure(cancelOrderResult.Error);

            outboxMessageRepository.Add(new OrderCancelledEvent
            {
                Id = Guid.CreateVersion7(),
                OccurredAt = utcNow,
                OrderId = order.Id,
                UserId = order.UserId,
                CancelledOrderItems = [.. order.Items.Select(i => new CancelledOrderItemDto
                {
                    ProductVariantId = i.VariantId, 
                    Quantity = i.Quantity
                })]
            }, utcNow);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
