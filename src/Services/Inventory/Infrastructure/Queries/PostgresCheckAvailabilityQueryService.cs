using Microsoft.EntityFrameworkCore;
using Vendora.Services.Inventory.Application.InventoryItems.CheckAvailability;
using Vendora.Services.Inventory.Infrastructure.Persistence;

namespace Vendora.Services.Inventory.Infrastructure.Queries;

public sealed class PostgresCheckAvailabilityQueryService(PostgresDbContext context) : ICheckAvailabilityQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        return await context.InventoryItems
            .Select(item => new Response()
            {
                ProductId = item.ProductId,
                Available = item.IsAvailable,
                AvailableQuantity = item.AvailableQuantity
            })
            .SingleOrDefaultAsync(
                item => item.ProductId == query.ProductId, 
                cancellationToken);
    }
}