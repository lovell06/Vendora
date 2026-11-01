namespace Vendora.Services.Ordering.Application.Orders.Get;

public sealed class Response
{
    public required OrderDetailDto Order { get; init; }
}

public sealed class OrderDetailDto
{
    public required long Id { get; init; }
    public required string Status { get; init; }
    public required string Currency { get; init; }
    public required decimal TotalAmount { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required IReadOnlyList<OrderItemDto> Items { get; init; }
}

public sealed class OrderItemDto
{
    public required long ProductId { get; init; }
    public required string ProductName { get; init; }
    public required decimal UnitPrice { get; init; }
    public required int Quantity { get; init; }
    public required decimal LineTotal { get; init; }
}