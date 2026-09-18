namespace Vendora.Services.Catalog.Application.Categories.Get;

public interface IGetCategoryQueryService
{
    Task<Response?> GetByIdAsync(int id, CancellationToken cancellationToken);
}