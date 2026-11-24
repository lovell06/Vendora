namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class EfCoreGetVariantQueryService(ApplicationDbContext context) : GetProductVariant.IQueryService
{
    public async Task<GetProductVariant.Response?> ExecuteAsync(GetProductVariant.Query query, CancellationToken ct)
    {
        var productVariant = await context.ProductVariants
            .AsNoTracking()
            .Where(variant => variant.Id == query.ProductVariantId)
            .Select(variant => new GetProductVariant.ProductVariantDto(
                variant.ProductId,
                variant.Id,
                variant.Sku))
            .SingleOrDefaultAsync(ct);

        return productVariant is null
            ? null
            : new GetProductVariant.Response(productVariant);
    }
}