using Vendora.Services.Inventory.Application.InventoryItems.Get;

namespace Vendora.Services.Inventory.Infrastructure.Queries;

public sealed class EfCoreGetInventoryItemQueryService(ApplicationDbContext context) : IGetInventoryItemQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        return await context.InventoryItems
            .Select(item => new Response
            {
                ProductId = item.ProductId,
                OnHandQuantity = item.OnHandQuantity,
                ReservedQuantity = item.ReservedQuantity,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            })
            .SingleOrDefaultAsync(item => item.ProductId == query.ProductId, cancellationToken);
    }
}