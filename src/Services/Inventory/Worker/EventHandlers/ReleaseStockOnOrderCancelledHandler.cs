namespace Vendora.Services.Inventory.Worker.EventHandlers;

public sealed class ReleaseStockOnOrderCancelledHandler(ISender sender) 
    : IIntegrationEventHandler<OrderCancelledEvent>
{
    public async Task HandleAsync(OrderCancelledEvent @event, CancellationToken ct)
    {
        var cmd = new ReleaseStock.Command(
            [
                .. @event.CancelledOrderItems.Select(i => new ReleaseStock.ItemDto(
                    i.ProductVariantId,
                    i.Quantity))
            ]);

        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            throw new Exception($"{result.Error.Code}. {result.Error.Message}"); 
    }
}