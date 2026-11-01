namespace Vendora.Services.Ordering.Application.Orders.List;

public static class Errors
{
    public static readonly Error InvalidPageNumber = new()
    {
        Code = "listing_orders.invalid_page_number",
        Message = "Page number must be greater than zero.",
        Type = ErrorType.Validation
    };

    public static readonly Error InvalidPageSize = new()
    {
        Code = "listing_orders.invalid_page_number",
        Message = "Page size must be greater than zero.",
        Type = ErrorType.Validation
    };
}