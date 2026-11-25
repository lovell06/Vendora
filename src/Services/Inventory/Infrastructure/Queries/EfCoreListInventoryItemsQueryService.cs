namespace Vendora.Services.Inventory.Infrastructure.Queries;

public sealed class EfCoreListInventoryItemsQueryService(ApplicationDbContext context) : ListInventoryItems.IQueryService
{
    public async Task<ListInventoryItems.Response> ExecuteAsync(ListInventoryItems.Query query, CancellationToken cancellationToken)
    {
        var efCoreQuery = context.InventoryItems
            .AsNoTracking()
            .OrderByDescending(item => item.CreatedAt)
            .ThenByDescending(item => item.Id)
            .Select(item => new ListInventoryItems.InventoryItemDto(
                item.Id,
                item.ProductId,
                item.ProductVariantId,
                item.Sku,
                item.OnHandQuantity,
                item.ReservedQuantity));

        var totalCount = await efCoreQuery.CountAsync(cancellationToken);

        var inventoryItems = await efCoreQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ToListAsync(cancellationToken);

        return new ListInventoryItems.Response(
            inventoryItems,
            query.Page,
            totalCount);
    }
}