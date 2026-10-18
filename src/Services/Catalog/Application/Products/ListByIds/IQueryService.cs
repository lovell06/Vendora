namespace Vendora.Services.Catalog.Application.Products.ListByIds;

public interface IQueryService
{
    Task<Response> ExecuteAsync(Query query, CancellationToken ct);
}
