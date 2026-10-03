using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Order.Domain.Orders;

public class Order
{
    public long Id { get; init; }
    public Guid UserId { get; init; }
    public OrderStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? PaidAt { get; private set; }
    public required string Currency { get; init; }

    private List<OrderItem> _items = [];
    public IReadOnlyList<OrderItem> Items => _items;

    public decimal TotalAmount => _items.Sum(item => item.SubTotal);

    private Order()
    {
    }

    public static Result<Order> Place(
        Guid userId,
        List<OrderItem> items,
        string currency,
        DateTimeOffset createdAt)
    {
        if (items.Count == 0)
            return Result<Order>.Failure(OrderErrors.EmptyItems);

        return Result<Order>.Success(new Order
        {
            Id = 0,
            UserId = userId,
            CreatedAt = createdAt,
            _items = items,
            Currency = currency,
            Status = OrderStatus.Pending
        });
    }

    public Result AddItem(OrderItem item, DateTimeOffset updatedAt)
    {
        if (_items.SingleOrDefault(i =>
                i.OrderId == item.OrderId &&
                i.ProductId == item.ProductId) is not null)
        {
            return Result.Failure(OrderErrors.CannotAddItem);
        }

        _items.Add(item);
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result RemoveItem(OrderItem item, DateTimeOffset updatedAt)
    {
        if (_items.Remove(item))
            UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result Confirm(DateTimeOffset confirmedAt)
    {
        if (Status is not OrderStatus.Pending &&
            Status is not OrderStatus.Confirmed)
        {
            return Result.Failure(OrderErrors.CannotConfirm);
        }

        if (Status is OrderStatus.Confirmed)
            return Result.Success();

        Status = OrderStatus.Confirmed;
        UpdatedAt = confirmedAt;

        return Result.Success();
    }

    public Result Prepare(DateTimeOffset prepareAt)
    {
        if (Status is OrderStatus.Preparing)
            return Result.Success();

        if (Status is not OrderStatus.Confirmed)
            return Result.Failure(OrderErrors.CannotPrepare);

        Status = OrderStatus.Preparing;
        UpdatedAt = prepareAt;

        return Result.Success();
    }

    public Result Delivery(DateTimeOffset deliveryAt)
    {
        if (Status is OrderStatus.Delivering)
            return Result.Success();

        if (Status is not OrderStatus.Preparing)
            return Result.Failure(OrderErrors.CannotDelivery);

        Status = OrderStatus.Delivering;
        UpdatedAt = deliveryAt;

        return Result.Success();
    }

    public Result Complete(DateTimeOffset completedAt)
    {
        if (Status is OrderStatus.Completed)
            return Result.Success();

        if (Status is not OrderStatus.Delivering)
            return Result.Failure(OrderErrors.CannotComplete);

        Status = OrderStatus.Completed;
        UpdatedAt = completedAt;

        return Result.Success();
    }

    public Result Cancel(DateTimeOffset cancelledAt)
    {
        if (Status is OrderStatus.Cancelled)
            return Result.Success();

        if (Status is not OrderStatus.Preparing &&
            Status is not OrderStatus.Confirmed &&
            Status is not OrderStatus.Pending)
        {
            return Result.Failure(OrderErrors.CannotCancel);
        }

        Status = OrderStatus.Cancelled;
        UpdatedAt = cancelledAt;

        return Result.Success();
    }

    public Result MaskAsPaid(DateTimeOffset paidAt)
    {
        PaidAt = paidAt;
        UpdatedAt = paidAt;

        return Result.Success();
    }
}
