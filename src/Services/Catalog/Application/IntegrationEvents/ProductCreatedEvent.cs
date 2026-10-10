namespace Vendora.Services.Catalog.Application.IntegrationEvents;

public sealed class ProductCreatedEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTimeOffset OccurredAt { get; init; }
    public long ProductId { get; init; }
}