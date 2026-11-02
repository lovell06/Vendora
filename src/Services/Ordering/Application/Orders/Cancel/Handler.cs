namespace Vendora.Services.Ordering.Application.Orders.Cancel;

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

        outboxMessageRepository.Add(new OrderCancelledEvent
        {
            Id = Guid.CreateVersion7(),
            OccurredAt = utcNow,
            OrderId = order.Id,
            UserId = order.UserId,
            CancelledOrderItems = [.. order.Items.Select(i => new CancelledOrderItemDto
            {
                ProductId = i.ProductId, 
                Quantity = i.Quantity
            })]
        }, utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return cancelOrderResult.IsFailure 
            ? Result.Failure(cancelOrderResult.Error) 
            : Result.Success();
    }
}