namespace Vendora.Services.Inventory.Worker.EventHandlers;

public sealed class InitializeOnProductCreatedHandler(ISender sender) : IIntegrationEventHandler<ProductVariantAddedEvent>
{
    public async Task HandleAsync(ProductVariantAddedEvent @event, CancellationToken ct)
    {
        var cmd = new InitializeInventoryItem.Command(
            @event.ProductId,
            @event.VariantId,
            @event.Sku);
        
        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            throw new Exception($"{result.Error.Code}. {result.Error.Message}");
    }
}
