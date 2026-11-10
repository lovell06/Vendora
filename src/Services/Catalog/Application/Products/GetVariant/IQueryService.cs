namespace Vendora.Services.Catalog.Application.Products.GetVariant;

public interface IQueryService
{
    Task<Response?> ExecuteAsync(Query query, CancellationToken ct);
}