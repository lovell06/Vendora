namespace Vendora.Services.Ordering.Domain.Orders;

public class OrderItem
{
    public long Id { get; init; }
    public long OrderId { get; init; }
    public long ProductId { get; init; }
    public long VariantId { get; init; }
    public required string Sku { get; init; }
    public required string ProductName { get; init; }
    public required string VariantName { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }

    public decimal SubTotal => Quantity * UnitPrice;

    private OrderItem()
    {
    }

    public static Result<OrderItem> Create(
        long productId, 
        long variantId,
        string sku,
        string productName,
        string variantName,
        int quantity,
        decimal unitPrice)
    {
        if (quantity <= 0)
            return Result<OrderItem>.Failure(OrderItemErrors.InvalidQuantity);
        
        if (unitPrice <= 0)
            return Result<OrderItem>.Failure(OrderItemErrors.InvalidUnitPrice);
        
        return Result<OrderItem>.Success(new OrderItem
        {
            Id = 0,
            ProductId = productId,
            VariantId = variantId,
            Sku = sku,
            ProductName = productName,
            VariantName = variantName,
            Quantity = quantity,
            UnitPrice = unitPrice,
        });
    }
}