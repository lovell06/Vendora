namespace Vendora.Services.Inventory.Application.InventoryItems.List;

public interface IListInventoryItemsQueryService
{
    Task<Response> ExecuteAsync(Query query, CancellationToken cancellationToken);
}