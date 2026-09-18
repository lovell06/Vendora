using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Application.Products.List;
using Vendora.Services.Catalog.Infrastructure.Persistence;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class PostgresListProductsQueryService(PostgresDbContext context) : IListProductsQueryService
{
    public async Task<IReadOnlyList<ProductDto>> GetByPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await context.Products
            .OrderByDescending(product => product.CreatedAt)
            .ThenByDescending(product => product.Id)
            .Skip((pageNumber-1) * pageSize)
            .Take(pageSize)
            .Where(product => product.IsVisible && product.DeletedAt == null)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Currency = product.Currency
            })
            .ToListAsync(cancellationToken);
    }
}