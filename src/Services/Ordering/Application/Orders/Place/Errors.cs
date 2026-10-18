namespace Vendora.Services.Ordering.Application.Orders.Place;

public static class Errors
{
    public static readonly Error InvalidCurrencyTypeCount = new()
    {
        Code = "placing_order.currency_type_count",
        Message = "Order must not has more than one currency type.",
        Type = ErrorType.Validation
    };

    public static readonly Error ProductNotFound = new()
    {
        Code = "placing_order.product_not_found",
        Message = "Not found product.",
        Type = ErrorType.NotFound
    };
}