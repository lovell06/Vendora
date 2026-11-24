namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class EfCoreListProductsByIdsQueryService(ApplicationDbContext context) : ListProductsByIds.IQueryService
{
    public async Task<ListProductsByIds.Response> ExecuteAsync(ListProductsByIds.Query query, CancellationToken ct)
    {
        return new ListProductsByIds.Response(
            await context.ProductVariants
                .AsNoTracking()
                .Where(variant => query.ProductVariantReferences.Contains(variant.Id))
                .Select(variant => new ListProductsByIds.ProductVariantDto(
                    variant.Product!.Id,
                    variant.Id,
                    variant.Product!.Name,
                    variant.Name,
                    variant.Sku,
                    variant.Price,
                    variant.Product!.Currency))
                .ToListAsync(ct));
    }
}