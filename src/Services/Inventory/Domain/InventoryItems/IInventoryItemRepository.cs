namespace Vendora.Services.Inventory.Domain.InventoryItems;

public interface IInventoryItemRepository
{
    Task<InventoryItem?> GetAsync(long productId, CancellationToken cancellationToken);
    Task<List<InventoryItem>> ListAsync(CancellationToken cancellationToken);
    Task<List<InventoryItem>> ListAsync(IReadOnlyList<long> productIds, CancellationToken cancellationToken);
    void Add(InventoryItem inventoryItem);
    Task<bool> ExistsByProductId(long productId, CancellationToken cancellationToken);
}