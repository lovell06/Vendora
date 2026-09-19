namespace Vendora.Services.Catalog.Application.Categories.List;

public interface IListCategoryQueryService
{
    Task<Response> ExecuteAsync(
        Query query,
        CancellationToken cancellationToken);
}