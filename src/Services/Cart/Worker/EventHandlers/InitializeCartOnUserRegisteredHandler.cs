using MediatR;
using Vendora.Services.Cart.Application.Carts.Initialize;
using Vendora.Services.Cart.Application.IntegrationEvents;

namespace Vendora.Services.Cart.Worker.EventHandlers;

public sealed class InitializeCartOnUserRegisteredHandler(ISender sender) 
    : IIntegrationEventHandler<UserRegisteredEvent>
{
    public async Task HandleAsync(UserRegisteredEvent @event, CancellationToken ct)
    {
        var cmd = new Command { UserId = @event.UserId };

        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            throw new Exception($"{result.Error.Code}. {result.Error.Message}");
    }
}