namespace Vendora.Services.Catalog.Application.Products.List;

public interface IListProductsQueryService
{
    Task<Response?> ExecuteAsync(
        Query query,
        CancellationToken cancellationToken);
}