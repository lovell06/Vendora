using Vendora.Services.Catalog.Application.Products.GetVariant;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class EfCoreGetVariantQueryService(ApplicationDbContext context) : IGetProductVariantQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken ct)
    {
        var productVariant = await context.ProductVariants
            .AsNoTracking()
            .Select(variant => new ProductVariantDto
            {
                ProductId = variant.ProductId,
                ProductVariantId = variant.Id,
                Sku = variant.Sku
            })
            .SingleOrDefaultAsync(dto => dto.ProductVariantId == query.ProductVariantId, ct);

        return productVariant is null
            ? null
            : new Response { ProductVariant = productVariant };
    }
}