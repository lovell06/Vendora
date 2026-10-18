using Vendora.Services.Catalog.Application.Products.ListByIds;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class EfCoreListProductsByIdsQueryService(ApplicationDbContext context) : IListProductsByIdsQueryService
{
    public async Task<Response> ExecuteAsync(Query query, CancellationToken ct)
    {
        return new Response
        {
            Products = await context.Products
                .AsNoTracking()
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Currency = p.Currency
                })
                .Where(p => query.ProductIds.Contains(p.Id))
                .ToListAsync(ct)
        };
    }
}