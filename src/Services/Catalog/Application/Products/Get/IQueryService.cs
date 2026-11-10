namespace Vendora.Services.Catalog.Application.Products.Get;

public interface IQueryService
{
    Task<ProductDto?> ExecuteAsync(Query query, CancellationToken cancellationToken);
}