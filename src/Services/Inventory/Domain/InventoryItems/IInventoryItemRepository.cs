namespace Vendora.Services.Inventory.Domain.InventoryItems;

public interface IInventoryItemRepository
{
    Task<InventoryItem?> GetAsync(long productId, CancellationToken cancellationToken);
    Task<ICollection<InventoryItem>> ListAsync(CancellationToken cancellationToken);
    void Add(InventoryItem inventoryItem);
    Task<bool> ExistsByProductId(long productId, CancellationToken cancellationToken);
}