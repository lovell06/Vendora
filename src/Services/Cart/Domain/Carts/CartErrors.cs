namespace Vendora.Services.Cart.Domain.Carts;

public static class CartErrors
{
    public static Error QuantityMustBePositive => new Error
    {
        Code = "cart.quantity_must_be_positive",
        Message = "Cart must be positive value.",
        Type = ErrorType.Validation
    };

    public static Error ProductNotExistsInCart => new Error
    {
        Code = "cart.product_not_exists_in_cart",
        Message = "Product not exists in cart.",
        Type = ErrorType.NotFound
    };

    public static Error InvalidProductVariantId = Error.Validation(
        code: "cart_item.invalid_product_Variant_id",
        message: "Invalid product variant ID.");
}