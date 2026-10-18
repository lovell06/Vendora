namespace Vendora.Services.Cart.Application.Carts.GetSelectedItems;

public static class Errors
{
    public static readonly Error UserNotFound = new()
    {
        Code = "getting_selected_item.user_not_found",
        Message = "The user has not initialized a cart.",
        Type = ErrorType.NotFound
    };

    public static readonly Error SelectedItemsIsEmpty = new()
    {
        Code = "getting_selected_item.selected_items_is_empty",
        Message = "The selected cart items must be not empty.",
        Type = ErrorType.Validation
    };

    public static readonly Error ProductsNotFoundInCart = new()
    {
        Code = "getting_selected_item.product_not_found_in_cart",
        Message = "Some products not found in cart.",
        Type = ErrorType.NotFound
    };
}