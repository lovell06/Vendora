using InitializeCommand = Vendora.Services.Inventory.Application.InventoryItems.Initialize.Command;

namespace Vendora.Services.Inventory.Worker.EventHandlers;

public sealed class InitializeOnProductCreatedHandler(ISender sender) : IIntegrationEventHandler<ProductCreatedEvent>
{
    public async Task HandleAsync(ProductCreatedEvent @event, CancellationToken ct)
    {
        var cmd = new InitializeCommand { ProductId = @event.ProductId };
        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            throw new Exception($"{result.Error.Code}. {result.Error.Message}");
    }
}
