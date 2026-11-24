namespace Vendora.Services.Inventory.Infrastructure.Queries;

public sealed class EfCoreGetInventoryItemQueryService(ApplicationDbContext context) : GetInventoryItem.IQueryService
{
    public async Task<GetInventoryItem.Response?> ExecuteAsync(GetInventoryItem.Query query, CancellationToken cancellationToken)
    {
        return await context.InventoryItems
            .Where(item => item.ProductVariantId == query.ProductVariantId)
            .Select(item => new GetInventoryItem.Response(
                item.ProductVariantId,
                item.OnHandQuantity,
                item.ReservedQuantity,
                item.CreatedAt,
                item.UpdatedAt))
            .SingleOrDefaultAsync(cancellationToken);
    }
}