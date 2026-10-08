namespace Vendora.BuildingBlocks.Messaging.Dispatching;

public sealed class IntegrationEventDispatcher(
    IEventTypeRegistry registry,
    IServiceProvider provider) : IIntegrationEventDispatcher
{
    public async Task DispatchAsync(
        string eventName, 
        ReadOnlyMemory<byte> body, 
        CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventName);

        var clrType = registry[eventName];

        if (!clrType.IsClass ||
            !typeof(IIntegrationEvent).IsAssignableFrom(clrType))
        {
            throw new InvalidOperationException(
                $"{clrType.FullName} must be a class implement {nameof(IIntegrationEvent)}");
        }

        var message = JsonSerializer.Deserialize<IntegrationMessage>(body.Span)
                      ?? throw new JsonException("Cannot deserialize integration message.");

        var @event = JsonSerializer.Deserialize(message.Payload, clrType)
                     ?? throw new JsonException($"Cannot deserialize event: \'{eventName}\'.");

        var handlers = provider.GetServices(
            typeof(IIntegrationEventHandler<>).MakeGenericType(clrType)).ToArray();

        if (handlers.Length == 0)
            throw new InvalidOperationException(
                $"No handler registered for event: \'{eventName}\'.");
        
        foreach (var handler in handlers)
        {
            if (handler is null)
                throw new InvalidOperationException(
                    $"Resolved a null handler for event \'{eventName}\'.");

            ct.ThrowIfCancellationRequested();

            await ((dynamic)handler).HandleAsync((dynamic)@event, ct);
        }
    }
}