namespace Vendora.Services.Catalog.Infrastructure.Repositories;

public class EfCoreProductRepository(ApplicationDbContext context) : IProductRepository
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