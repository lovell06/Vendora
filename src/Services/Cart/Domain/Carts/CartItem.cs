using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Cart.Domain.Carts;

public class CartItem
{
    public long CartId { get; init; }
    public long ProductId { get; init; }
    public int Quantity { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private CartItem()
    {
    }

    public static CartItem Create(
        long cartId,
        long productId,
        int quantity,
        DateTimeOffset createdAt)
    {
        return new CartItem
        {
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity,
            CreatedAt = createdAt
        };
    }

    public Result SetQuantity(int quantity, DateTimeOffset updatedAt)
    {
        if (quantity <= 0)
            return Result.Failure(CartErrors.QuantityMustBePositive);
        
        Quantity = quantity;
        UpdatedAt = updatedAt;
        return Result.Success();
    }
}