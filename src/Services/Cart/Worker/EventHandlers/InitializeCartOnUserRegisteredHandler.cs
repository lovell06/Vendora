using MediatR;
using Vendora.Services.Cart.Application.Carts;
using Vendora.Services.Cart.Application.IntegrationEvents;

namespace Vendora.Services.Cart.Worker.EventHandlers;

public sealed class InitializeCartOnUserRegisteredHandler(ISender sender) 
    : IIntegrationEventHandler<UserRegisteredEvent>
{
    public async Task HandleAsync(UserRegisteredEvent @event, CancellationToken ct)
    {
        var cmd = new InitializeCart.Command(@event.UserId);

        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            throw new Exception($"{result.Error.Code}. {result.Error.Message}");
    }
}