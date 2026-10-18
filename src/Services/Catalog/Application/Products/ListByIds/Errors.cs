namespace Vendora.Services.Catalog.Application.Products.ListByIds;

public static class Errors
{
    public static readonly Error ProductIdsEmpty = new()
    {
        Code = "listing_product_by_ids.product_ids_empty",
        Message = "At least one product ID is required.",
        Type = ErrorType.Validation
    };

    public static readonly Error SomeProductsNotFound = new()
    {
        Code = "listing_product_by_ids.some_products_not_found",
        Message = "One or more requested products were not found.",
        Type = ErrorType.NotFound
    };
}