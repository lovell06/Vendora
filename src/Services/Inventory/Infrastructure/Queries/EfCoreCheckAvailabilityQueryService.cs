using Vendora.Services.Inventory.Application.InventoryItems.CheckAvailability;

namespace Vendora.Services.Inventory.Infrastructure.Queries;

public sealed class EfCoreCheckAvailabilityQueryService(ApplicationDbContext context) : ICheckAvailabilityQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        return await context.InventoryItems
            .Select(item => new Response()
            {
                ProductVariantId = item.ProductVariantId,
                IsAvailable = item.IsAvailable,
                AvailableQuantity = item.AvailableQuantity
            })
            .SingleOrDefaultAsync(
                item => item.ProductVariantId == query.ProductVariantId, 
                cancellationToken);
    }
}