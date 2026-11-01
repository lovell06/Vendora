namespace Vendora.Services.Ordering.Application.Orders.List;

public sealed class Response
{
    public required IReadOnlyList<OrderSummaryDto> Items { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
    public required int TotalCount { get; init; }
}

public sealed class OrderSummaryDto
{
    public required long Id { get; init; }
    public required string Status { get; init; }
    public required string Currency { get; init; }
    public required decimal TotalAmount { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}