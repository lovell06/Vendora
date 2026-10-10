using Vendora.BuildingBlocks.Messaging.Abstractions;

namespace Vendora.Services.Inventory.Application.IntegrationEvents;

public sealed class ProductCreatedEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTimeOffset OccurredAt { get; init; }
    public long ProductId { get; init; }
}
