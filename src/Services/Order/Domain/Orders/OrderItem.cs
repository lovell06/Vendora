using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Order.Domain.Orders;

public class OrderItem
{
    public long OrderId { get; init; }
    public long ProductId { get; init; }
    public required string ProductName { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }

    public decimal SubTotal => Quantity * UnitPrice;

    private OrderItem()
    {
    }

    public static Result<OrderItem> Create(
        long orderId, 
        long productId, 
        string productName,
        int quantity,
        decimal unitPrice)
    {
        if (quantity <= 0)
            return Result<OrderItem>.Failure(OrderItemErrors.InvalidQuantity);
        
        if (unitPrice <= 0)
            return Result<OrderItem>.Failure(OrderItemErrors.InvalidUnitPrice);
        
        return Result<OrderItem>.Success(new OrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice,
        });
    }
}