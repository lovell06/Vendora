using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Order.Domain.Orders;

public static class OrderItemErrors
{
    public static readonly Error InvalidQuantity = new Error
    {
        Code = "order_item.invalid_quantity",
        Message = "Quantity must be greater than zero.",
        Type = ErrorType.Validation
    };

    public static readonly Error InvalidUnitPrice = new Error
    {
        Code = "order_item.invalid_unit_price",
        Message = "Unit price must be greater than zero.",
        Type = ErrorType.Validation
    };
}