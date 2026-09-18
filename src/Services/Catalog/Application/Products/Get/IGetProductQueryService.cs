namespace Vendora.Services.Catalog.Application.Products.Get;

public interface IGetProductQueryService
{
    Task<Response?> GetProductById(long id, CancellationToken cancellationToken);
}