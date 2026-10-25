namespace Vendora.Services.Cart.Application.IntegrationEvents;

public sealed class OrderPlacedEvent : IIntegrationEvent
{
    public required Guid Id { get; init; }
    public required DateTimeOffset OccurredAt { get; init; }
    public required long OrderId { get; init; }
    public required Guid UserId { get; init; }
    public required IReadOnlyList<long> Items { get; init; }
}