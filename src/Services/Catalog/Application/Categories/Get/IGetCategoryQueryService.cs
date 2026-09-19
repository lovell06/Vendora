namespace Vendora.Services.Catalog.Application.Categories.Get;

public interface IGetCategoryQueryService
{
    Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken);
}