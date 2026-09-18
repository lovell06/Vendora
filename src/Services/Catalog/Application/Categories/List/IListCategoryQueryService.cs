namespace Vendora.Services.Catalog.Application.Categories.List;

public interface IListCategoryQueryService
{
    Task<Response> GetByPageAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken);
}