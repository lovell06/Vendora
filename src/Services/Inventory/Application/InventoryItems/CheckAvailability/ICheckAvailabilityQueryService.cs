namespace Vendora.Services.Inventory.Application.InventoryItems.CheckAvailability;

public interface ICheckAvailabilityQueryService
{
    Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken);
}