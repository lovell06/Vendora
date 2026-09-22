using Microsoft.EntityFrameworkCore;
using Vendora.Services.Inventory.Application.InventoryItems.Get;
using Vendora.Services.Inventory.Infrastructure.Persistence;

namespace Vendora.Services.Inventory.Infrastructure.Queries;

public sealed class PostgresGetInventoryItemQueryService(PostgresDbContext context) : IGetInventoryItemQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        return await context.InventoryItems
            .Select(item => new Response
            {
                ProductId = item.ProductId,
                OnHandQuantity = item.OnHandQuantity,
                ReservedQuantity = item.ReservedQuantity
            })
            .SingleOrDefaultAsync(item => item.ProductId == query.ProductId, cancellationToken);
    }
}