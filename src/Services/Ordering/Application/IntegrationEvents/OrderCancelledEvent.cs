namespace Vendora.Services.Ordering.Application.IntegrationEvents;

public sealed class OrderCancelledEvent : IIntegrationEvent
{
    public Guid Id { get ; init; }
    public DateTimeOffset OccurredAt { get; init; }
    public required Guid UserId { get; init; }
    public required long OrderId { get; init; }
    public required IReadOnlyList<CancelledOrderItemDto> CancelledOrderItems { get; init; }
}

public sealed class CancelledOrderItemDto
{
    public required long ProductId { get; init; }
    public required long Quantity { get; init; }
}