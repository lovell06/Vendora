namespace Vendora.Services.Catalog.Application.Products.List;

public interface IListProductsQueryService
{
    Task<Response> GetByPageAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken);
}