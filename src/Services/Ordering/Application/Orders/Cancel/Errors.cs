namespace Vendora.Services.Ordering.Application.Orders.Cancel;

public static class Errors
{
    public static readonly Error OrderNotFound = new()
    {
        Code = "cancelling_order.order_not_found",
        Message = "Order not found.",
        Type = ErrorType.NotFound
    };
}