namespace Vendora.Services.Cart.Application.Carts.GetSelectedItems;

public sealed class Response
{
    public required List<CartItemDto> Items { get; init; }
}

public sealed class CartItemDto
{
    public long ProductId { get; init; }
    public int Quantity { get; init; }
}