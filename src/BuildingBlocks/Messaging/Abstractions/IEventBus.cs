namespace Vendora.BuildingBlocks.Messaging.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationMessage message, CancellationToken ct);
    Task SubscribeAsync<TEvent, THandler>(CancellationToken ct) 
        where TEvent : IIntegrationEvent 
        where THandler : IIntegrationEventHandler<TEvent>;
    Task UnsubscribeAsync<TEvent, THandler>(CancellationToken ct) 
        where TEvent : IIntegrationEvent 
        where THandler : IIntegrationEventHandler<TEvent>;
}