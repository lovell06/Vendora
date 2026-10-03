namespace Vendora.Services.Order.Domain.Orders;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Preparing,
    Delivering,
    Completed,
    Cancelled
}