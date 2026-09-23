using Microsoft.EntityFrameworkCore;
using Vendora.Services.Inventory.Application.InventoryItems.List;
using Vendora.Services.Inventory.Infrastructure.Persistence;

namespace Vendora.Services.Inventory.Infrastructure.Queries;

public sealed class PostgresListInventoryItemsQueryService(PostgresDbContext context) : IListInventoryItemsQueryService
{
    public async Task<Response> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        var efCoreQuery = context.InventoryItems
            .AsNoTracking()
            .Select(item => new InventoryItemDto
            {
                ProductId = item.ProductId,
                OnHandQuantity = item.OnHandQuantity,
                ReservedQuantity = item.ReservedQuantity
            });

        var totalCount = await efCoreQuery.CountAsync(cancellationToken);

        var inventoryItems = await efCoreQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ToListAsync(cancellationToken);

        return new Response
        {
            Page = query.Page,
            TotalCount = totalCount,
            InventoryItems = inventoryItems
        };
    }
}