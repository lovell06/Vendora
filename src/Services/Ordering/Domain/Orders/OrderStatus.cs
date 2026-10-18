namespace Vendora.Services.Ordering.Domain.Orders;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Preparing,
    Delivering,
    Completed,
    Cancelled
}