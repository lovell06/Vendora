namespace Vendora.Services.Inventory.Application.InventoryItems.Get;

public interface IGetInventoryItemQueryService
{
    Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken);
}