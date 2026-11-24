namespace Vendora.Services.Catalog.Infrastructure.Queries.Categories;

public sealed class EfCoreGetCategoryQueryService(ApplicationDbContext context) : GetCategory.IQueryService
{
    public async  Task<GetCategory.Response?> ExecuteAsync(GetCategory.Query query, CancellationToken cancellationToken)
    {
        return await context.Categories
            .AsNoTracking()
            .Where(category => category.DeletedAt == null)
            .Where(category => category.Id == query.Id)
            .Select(category => new GetCategory.Response(category.Id, category.Name))
            .SingleOrDefaultAsync(cancellationToken);
    }
}