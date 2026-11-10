namespace Vendora.Services.Inventory.Infrastructure.Repositories;

public sealed class EfCoreInventoryItemRepository(ApplicationDbContext context) : IInventoryItemRepository
{
    public async Task<List<InventoryItem>> ListAsync(IReadOnlyList<long> productVariantIds, CancellationToken cancellation)
    {
        return await context.InventoryItems
            .Where(item => productVariantIds.Contains(item.ProductVariantId))
            .ToListAsync(cancellation);
    }

    public void Add(InventoryItem item)
    {
        context.InventoryItems.Add(item);
    }

    public async Task<bool> ExistsByProductVariantId(long productId, CancellationToken cancellationToken)
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

    public async Task<List<InventoryItem>> ListAsync(CancellationToken cancellationToken)
    {
        return await context.InventoryItems.ToListAsync(cancellationToken);
    }
}