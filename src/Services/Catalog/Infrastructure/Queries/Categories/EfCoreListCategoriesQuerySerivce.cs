namespace Vendora.Services.Catalog.Infrastructure.Queries.Categories;

public sealed class EfCoreListCategoriesQuerySerivce(ApplicationDbContext context) : ListCategories.IQueryService
{
    public async Task<ListCategories.Response> ExecuteAsync(ListCategories.Query query, CancellationToken cancellationToken)
    {
        var categoryQuery = context.Categories
            .AsNoTracking()
            .Where(category => category.DeletedAt == null);

        var totalCount = await categoryQuery.CountAsync(cancellationToken);

        var categories = await categoryQuery
            .Skip((query.Page-1) * query.Size)
            .Take(query.Size)
            .Select(category => new ListCategories.CategoryDto(
                category.Id,
                category.Name))
            .ToListAsync(cancellationToken);

        return new ListCategories.Response(
            query.Page,
            totalCount,
            categories);
    }
}