namespace Vendora.BuildingBlocks.Messaging.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationMessage message, CancellationToken ct);
}