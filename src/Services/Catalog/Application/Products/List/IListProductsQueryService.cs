namespace Vendora.Services.Catalog.Application.Products.List;

public interface IListProductsQueryService
{
    Task<IReadOnlyList<ProductDto>> GetByPageAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken);
}