namespace Vendora.Services.Catalog.Infrastructure.Persistence.Helpers;

internal static class PrimaryProductImageHelper
{
    internal static async Task<int> PrepareAsync(DbContext context, CancellationToken ct)
    {
        var demoteImageIds = context.ChangeTracker
            .Entries<ProductImage>()
            .Where(entry =>
                entry.State == EntityState.Modified &&
                entry.Property(image => image.IsPrimary).IsModified &&
                entry.Property(image => image.IsPrimary).OriginalValue)
            .Select(entry => entry.Entity.Id)
            .ToHashSet();

        if (demoteImageIds.Count == 0)
            return 0;

        return await context.Set<ProductImage>()
            .Where(image => demoteImageIds.Contains(image.Id))
            .ExecuteUpdateAsync(setter =>
                setter.SetProperty(
                    image => image.IsPrimary,
                    image => false), ct);
    }
}