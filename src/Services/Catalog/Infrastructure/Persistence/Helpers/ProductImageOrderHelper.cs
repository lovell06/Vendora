namespace Vendora.Services.Catalog.Infrastructure.Persistence.Helpers;

internal static class ProductImageOrderHelper
{
    internal static async Task<int> PrepareAsync(DbContext context, CancellationToken ct)
    {
        var allEntries = context.ChangeTracker.Entries<ProductImage>().ToList();

        var changedImages = allEntries
            .Where(entry =>
                entry.State == EntityState.Modified &&
                entry.Property(image => image.SortOrder).IsModified)
            .OrderBy(entry => entry.Entity.ProductId)
            .ThenBy(entry => entry.Entity.SortOrder)
            .ToList();

        if (changedImages.Count == 0)
            return 0;

        if (context.Database.CurrentTransaction is null)
            throw new InvalidOperationException("First, open a Transaction.");

        var totalAffected = 0;

        foreach (var group in changedImages.GroupBy(entry => entry.Entity.ProductId))
        {
            var productId = group.Key;
            var images = context.Set<ProductImage>()
                .Where(image => image.ProductId == productId);

            var maxStoredOrder = await images
                .MaxAsync(image => (int?)image.SortOrder, ct) ?? 0;

            var maxPendingOrder = allEntries
                .Where(entry => entry.Entity.ProductId == productId)
                .Max(entry => entry.Entity.SortOrder);

            var temporaryOrder = Math.Max(maxStoredOrder, maxPendingOrder);

            foreach (var entry in group.OrderBy(entry => entry.Entity.Id))
            {
                var imageId = entry.Entity.Id;
                var order = entry.Property(image => image.SortOrder);
                var expectedOrder = order.OriginalValue;
                temporaryOrder = checked(temporaryOrder + 1);

                var affected = await images
                    .Where(image => image.Id == imageId && image.SortOrder == expectedOrder)
                    .ExecuteUpdateAsync(
                        setters => setters.SetProperty(image => image.SortOrder, temporaryOrder),
                        ct);

                if (affected != 1)
                    throw new DbUpdateConcurrencyException(
                        $"Sort order for product image {imageId} changed.");

                order.OriginalValue = temporaryOrder;
                order.IsModified = true;
                totalAffected += affected;
            }
        }
        
        return totalAffected;
    }
}