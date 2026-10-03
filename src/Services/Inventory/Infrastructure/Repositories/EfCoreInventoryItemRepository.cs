namespace Vendora.Services.Inventory.Infrastructure.Repositories;

public sealed class EfCoreInventoryItemRepository(ApplicationDbContext context) : IInventoryItemRepository
{
    public void Add(InventoryItem item)
    {
        context.InventoryItems.Add(item);
    }

    public async Task<bool> ExistsByProductId(long productId, CancellationToken cancellationToken)
    {
        return await context.InventoryItems
            .SingleOrDefaultAsync(
                item => item.ProductId == productId, 
                cancellationToken) is not null;
    }

    public async Task<InventoryItem?> GetAsync(long productId, CancellationToken cancellationToken)
    {
        return await context.InventoryItems
            .SingleOrDefaultAsync(item => item.ProductId == productId, cancellationToken);
    }

    public async Task<ICollection<InventoryItem>> ListAsync(CancellationToken cancellationToken)
    {
        return await context.InventoryItems.ToListAsync(cancellationToken);
    }
}