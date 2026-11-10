namespace Vendora.Services.Inventory.Domain.InventoryItems;

public interface IInventoryItemRepository
{
    Task<InventoryItem?> GetAsync(long inventoryItemId, CancellationToken cancellationToken);
    Task<List<InventoryItem>> ListAsync(CancellationToken cancellationToken);
    Task<List<InventoryItem>> ListAsync(IReadOnlyList<long> productVariantIds, CancellationToken cancellationToken);
    void Add(InventoryItem inventoryItem);
    Task<bool> ExistsByProductVariantId(long productVariantId, CancellationToken cancellationToken);
}