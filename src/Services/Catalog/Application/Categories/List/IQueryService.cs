namespace Vendora.Services.Catalog.Application.Categories.List;

public interface IQueryService
{
    Task<Response> ExecuteAsync(
        Query query,
        CancellationToken cancellationToken);
}