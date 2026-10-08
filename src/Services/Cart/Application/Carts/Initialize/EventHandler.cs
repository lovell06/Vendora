namespace Vendora.Services.Cart.Application.Carts.Initialize;

public sealed class EventHandler(ISender sender) : IIntegrationEventHandler<UserRegisteredEvent>
{
    public async Task HandleAsync(UserRegisteredEvent @event, CancellationToken ct)
    {
        var cmd = new Command { UserId = @event.UserId };

        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            throw new Exception($"{result.Error.Code}. {result.Error.Message}");
    }
}