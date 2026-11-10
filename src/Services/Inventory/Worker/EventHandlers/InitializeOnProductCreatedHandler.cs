using InitializeCommand = Vendora.Services.Inventory.Application.InventoryItems.Initialize.Command;

namespace Vendora.Services.Inventory.Worker.EventHandlers;

public sealed class InitializeOnProductCreatedHandler(ISender sender) : IIntegrationEventHandler<ProductVariantAddedEvent>
{
    public async Task HandleAsync(ProductVariantAddedEvent @event, CancellationToken ct)
    {
        var cmd = new InitializeCommand
        {
            ProductId = @event.ProductId,
            ProductVariantId = @event.VariantId,
            Sku = @event.Sku
        };
        
        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            throw new Exception($"{result.Error.Code}. {result.Error.Message}");
    }
}
