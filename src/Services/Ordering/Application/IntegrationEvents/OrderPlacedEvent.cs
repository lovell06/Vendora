namespace Vendora.Services.Ordering.Application.IntegrationEvents;

public sealed class OrderPlacedEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTimeOffset OccurredAt { get; init; }
    public long OrderId { get; init; }
}