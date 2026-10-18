namespace Vendora.Services.Catalog.Application.Products.List;

public interface IQueryService
{
    Task<Response?> ExecuteAsync(
        Query query,
        CancellationToken cancellationToken);
}