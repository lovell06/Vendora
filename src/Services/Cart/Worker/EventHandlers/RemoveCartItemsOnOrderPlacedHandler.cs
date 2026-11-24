using MediatR;
using Vendora.Services.Cart.Application.Carts;
using Vendora.Services.Cart.Application.IntegrationEvents;

namespace Vendora.Services.Cart.Worker.EventHandlers;

public sealed class RemoveCartItemsOnOrderPlacedHandler(
    ISender sender) 
    : IIntegrationEventHandler<OrderPlacedEvent>
{
    public async Task HandleAsync(OrderPlacedEvent @event, CancellationToken ct)
    {
        var cmd = new RemoveOrderedItems.Command(
            @event.UserId,
            @event.Items);

        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            throw new Exception($"{result.Error.Code}. {result.Error.Message}");
    }
}