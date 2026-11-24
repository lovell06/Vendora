namespace Vendora.Services.Inventory.Infrastructure.Queries;

public sealed class EfCoreCheckAvailabilityQueryService(ApplicationDbContext context) : CheckInventoryAvailability.IQueryService
{
    public async Task<CheckInventoryAvailability.Response?> ExecuteAsync(CheckInventoryAvailability.Query query, CancellationToken cancellationToken)
    {
        return await context.InventoryItems
            .Where(item => item.ProductVariantId == query.ProductVariantId)
            .Select(item => new CheckInventoryAvailability.Response(
                item.ProductVariantId,
                item.IsAvailable,
                item.AvailableQuantity))
            .SingleOrDefaultAsync(cancellationToken);
    }
}