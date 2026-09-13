namespace Vendora.Services.Catalog.Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken);
    void Add(Product product);
}