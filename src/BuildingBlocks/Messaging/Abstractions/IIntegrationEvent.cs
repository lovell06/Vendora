namespace Vendora.BuildingBlocks.Messaging.Abstractions;

public interface IIntegrationEvent
{
    Guid Id { get; init; }
    DateTimeOffset OccurredAt { get; init; }
}