using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Order.Domain.Orders;

public static class OrderErrors
{
    public static readonly Error EmptyItems = new Error
    {
        Code = "order.empty_items",
        Message = "An order must contain at least one item.",
        Type = ErrorType.Validation
    };

    public static readonly Error CannotAddItem = new Error
    {
        Code = "order.cannot_add_item",
        Message = "Item is already exists in order.",
        Type = ErrorType.Conflict
    };

    public static readonly Error CannotConfirm = new Error
    {
        Code = "order.cannot_confirm",
        Message = "The order cannot be confirmed in its current status.",
        Type = ErrorType.Conflict
    };

    public static readonly Error CannotPrepare = new Error
    {
        Code = "order.cannot_prepare",
        Message = "The order cannot be prepared in its current status.",
        Type = ErrorType.Conflict
    };

    public static readonly Error CannotDelivery = new Error
    {
        Code = "order.cannot_delivery",
        Message = "The order cannot be delivered in its current status.",
        Type = ErrorType.Conflict
    };

    public static readonly Error CannotComplete = new Error
    {
        Code = "order.cannot_complete",
        Message = "The order cannot be completed in its current status.",
        Type = ErrorType.Conflict
    };

    public static readonly Error CannotCancel = new Error
    {
        Code = "order.cannot_cancel",
        Message = "The order cannot be cancelled in its current status.",
        Type = ErrorType.Conflict
    };
}