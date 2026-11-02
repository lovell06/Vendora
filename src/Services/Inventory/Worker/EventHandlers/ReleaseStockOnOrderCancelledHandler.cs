using ReleaseStockCommand = Vendora.Services.Inventory.Application.InventoryItems.ReleaseStock.Command;

namespace Vendora.Services.Inventory.Worker.EventHandlers;

public sealed class ReleaseStockOnOrderCancelledHandler(ISender sender) 
    : IIntegrationEventHandler<OrderCancelledEvent>
{
    public async Task HandleAsync(OrderCancelledEvent @event, CancellationToken ct)
    {
        var cmd = new ReleaseStockCommand
        {
            Items =
            [
                .. @event.CancelledOrderItems.Select(i => new ItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                })
            ]
        };

        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            throw new Exception($"{result.Error.Code}. {result.Error.Message}"); 
    }
}