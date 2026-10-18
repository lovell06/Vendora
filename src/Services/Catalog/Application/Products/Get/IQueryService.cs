namespace Vendora.Services.Catalog.Application.Products.Get;

public interface IQueryService
{
    Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken);
}