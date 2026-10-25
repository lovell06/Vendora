namespace Vendora.Services.Cart.Application.Carts.RemoveOrderedItems;

public static class Errors
{
    public static readonly Error UserNotFound = new()
    {
        Code = "removing_ordered_items.user_not_found",
        Message = "The user has not initialized a cart.",
        Type = ErrorType.NotFound
    };
}