namespace Vendora.Services.Catalog.Infrastructure.Persistence.Helpers;

internal static class PrimaryProductVariantImageHelper
{
    internal static async Task<int> PrepareAsync(DbContext context, CancellationToken ct)
    {
        var variantImageIds = context.ChangeTracker
            .Entries<ProductVariantImage>()
            .Where(entry =>
                entry.State == EntityState.Modified &&
                entry.Property(image => image.IsPrimary).IsModified && 
                entry.Property(image => image.IsPrimary).OriginalValue)
            .Select(entry => entry.Entity.Id)
            .ToHashSet();

        if (variantImageIds.Count == 0)
            return 0;

        return await context.Set<ProductVariantImage>()
            .Where(image => variantImageIds.Contains(image.Id))
            .ExecuteUpdateAsync(setter =>
                setter.SetProperty(
                    image => image.IsPrimary,
                    image => false), ct);
    }
}