using Vendora.Services.Catalog.Application.Categories.Get;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Categories;

public sealed class EfCoreGetCategoryQueryService(ApplicationDbContext context) : IGetCategoryQueryService
{
    public async  Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        return await context.Categories
            .AsNoTracking()
            .Where(category => category.DeletedAt == null)
            .Select(category => new Response { Id = category.Id, Name = category.Name })
            .SingleOrDefaultAsync(category => category.Id == query.Id, cancellationToken);
    }
}