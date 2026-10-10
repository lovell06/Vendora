namespace Vendora.Services.Inventory.Application.InventoryItems.Initialize;

public sealed class EventHandler(ISender sender) : IIntegrationEventHandler<ProductCreatedEvent>
{
    public async Task HandleAsync(ProductCreatedEvent @event, CancellationToken ct)
    {
        var cmd = new Command { ProductId = @event.ProductId };
        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            throw new Exception($"{result.Error.Code}. {result.Error.Message}");
    }
}
