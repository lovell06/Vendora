namespace Vendora.BuildingBlocks.Messaging.Abstractions;

public interface IIntegrationEventDispatcher
{
    Task DispatchAsync(
        string eventName,
        ReadOnlyMemory<byte> body,
        CancellationToken ct);
}