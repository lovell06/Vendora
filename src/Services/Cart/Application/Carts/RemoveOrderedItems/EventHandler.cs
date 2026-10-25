namespace Vendora.Services.Cart.Application.Carts.RemoveOrderedItems;

public sealed class EventHandler(
    ISender sender,
    ILogger<EventHandler> logger) : IIntegrationEventHandler<OrderPlacedEvent>
{
    public async Task HandleAsync(OrderPlacedEvent @event, CancellationToken ct)
    {
        var cmd = new Command
        {
            UserId = @event.UserId,
            Items = @event.Items
        };

        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            logger.LogWarning(result.Error.Message);
    }
}