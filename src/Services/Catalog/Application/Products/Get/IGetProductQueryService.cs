namespace Vendora.Services.Catalog.Application.Products.Get;

public interface IGetProductQueryService
{
    Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken);
}