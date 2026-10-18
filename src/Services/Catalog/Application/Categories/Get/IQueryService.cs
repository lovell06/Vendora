namespace Vendora.Services.Catalog.Application.Categories.Get;

public interface IQueryService
{
    Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken);
}