using Vendora.Services.Catalog.Application.Products.ListByIds;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class EfCoreListProductsByIdsQueryService(ApplicationDbContext context) : IListProductsByIdsQueryService
{
    public async Task<Response> ExecuteAsync(Query query, CancellationToken ct)
    {
        return new Response
        {
            Products = await context.ProductVariants
                .AsNoTracking()
                .Where(variant => query.ProductVariantReferences.Contains(variant.Id))
                .Select(variant => new ProductVariantDto
                {
                    ProductId = variant.Product!.Id,
                    VariantId = variant.Id,
                    Sku = variant.Sku,
                    ProductName = variant.Product!.Name,
                    VariantName = variant.Name,
                    Price = variant.Price,
                    Currency = variant.Product!.Currency
                })
                .ToListAsync(ct)
        };
    }
}