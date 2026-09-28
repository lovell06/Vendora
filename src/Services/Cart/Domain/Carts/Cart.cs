using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Cart.Domain.Carts;

public class Cart
{
    public long Id { get; init; }
    public Guid UserId { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    private readonly List<CartItem> _items = [];
    public IReadOnlyList<CartItem> Items => _items;

    private Cart()
    {
    }

    public static Cart Create(Guid userId, DateTimeOffset createdAt)
    {
        return new Cart
        {
            Id = 0,
            UserId = userId,
            CreatedAt = createdAt
        };
    }

    public void AddItem(CartItem item, DateTimeOffset updatedAt)
    {
        _items.Add(item);
        UpdatedAt = updatedAt;
    }

    public void RemoveItem(CartItem item, DateTimeOffset updatedAt)
    {
        _items.Remove(item);
        UpdatedAt = updatedAt;
    }

    public Result SetItemQuantity(
        long productId, 
        int quantity,
        DateTimeOffset updatedAt)
    {
        var item = _items.SingleOrDefault(i => i.ProductId == productId);

        return item is not null
            ? item.SetQuantity(quantity, updatedAt)
            : Result.Failure(CartErrors.ProductNotExistsInCart);
    }
}