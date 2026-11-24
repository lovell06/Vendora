namespace Vendora.Services.Cart.Domain.Carts;

public class CartItem
{
    public long CartId { get; init; }
    public long ProductVariantId { get; init; }
    public int Quantity { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private CartItem()
    {
    }

    public static Result<CartItem> Create(
        long productVariantId,
        int quantity,
        DateTimeOffset createdAt)
    {
        if (productVariantId < 1)
            return Result<CartItem>.Failure(CartErrors.InvalidProductVariantId);
        
        if (quantity < 1)
            return Result<CartItem>.Failure(CartErrors.QuantityMustBePositive);
        
        return Result<CartItem>.Success(new CartItem
        {
            CartId = 0,
            ProductVariantId = productVariantId,
            Quantity = quantity,
            CreatedAt = createdAt
        });
    }

    public Result SetQuantity(int quantity, DateTimeOffset updatedAt)
    {
        if (quantity < 1)
            return Result.Failure(CartErrors.QuantityMustBePositive);
        
        Quantity = quantity;
        UpdatedAt = updatedAt;
        return Result.Success();
    }

    public Result IncreaseQuantity(int quantity, DateTimeOffset updatedAt)
    {
        if (quantity < 1)
            return Result.Failure(CartErrors.QuantityMustBePositive);
        
        Quantity += quantity;
        UpdatedAt = updatedAt;
        return Result.Success();
    }
}