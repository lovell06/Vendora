using Vendora.Services.Inventory.Application.InventoryItems.CheckAvailability;

namespace Vendora.Services.Inventory.Infrastructure.Queries;

public sealed class EfCoreCheckAvailabilityQueryService(ApplicationDbContext context) : ICheckAvailabilityQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        return await context.InventoryItems
            .Select(item => new Response()
            {
                ProductId = item.ProductId,
                IsAvailable = item.IsAvailable,
                AvailableQuantity = item.AvailableQuantity
            })
            .SingleOrDefaultAsync(
                item => item.ProductId == query.ProductId, 
                cancellationToken);
    }
}