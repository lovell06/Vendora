namespace Vendora.Services.Ordering.Domain.Orders;

public enum OrderStatus
{
    Pending,
    Rejected,
    Confirmed,
    Preparing,
    Delivering,
    Completed,
    Cancelled
}