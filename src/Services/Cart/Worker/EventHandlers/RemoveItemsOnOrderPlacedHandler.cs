using MediatR;
using Vendora.Services.Cart.Application.Carts.RemoveOrderedItems;
using Vendora.Services.Cart.Application.IntegrationEvents;

namespace Vendora.Services.Cart.Worker.EventHandlers;

public sealed class RemoveItemsOnOrderPlacedHandler(
    ISender sender) 
    : IIntegrationEventHandler<OrderPlacedEvent>
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
            throw new Exception($"{result.Error.Code}. {result.Error.Message}");
    }
}