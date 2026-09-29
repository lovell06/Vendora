namespace Vendora.Services.Cart.Application.Carts.ListItems;

public sealed class Response
{
    public required IReadOnlyList<ItemDto> Items { get; init; }
}

public sealed class ItemDto
{
    public long ProductId { get; init; }
    public int Quantity { get; init; }
}