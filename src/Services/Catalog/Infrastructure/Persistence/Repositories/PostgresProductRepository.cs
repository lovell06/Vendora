using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Infrastructure.Persistence.Repositories;

public class PostgresProductRepository(PostgresDbContext context) : IProductRepository
{
    public void Add(Product product)
    {
        context.Add(product);
    }

    public async Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await context.Products.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}