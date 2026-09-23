using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Application.Categories.List;
using Vendora.Services.Catalog.Infrastructure.Persistence;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Categories;

public sealed class PostgresListCategoriesQuerySerivce(PostgresDbContext context) : IListCategoryQueryService
{
    public async Task<Response> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        var categoryQuery = context.Categories
            .AsNoTracking()
            .Where(category => category.DeletedAt == null);

        var totalCount = await categoryQuery.CountAsync(cancellationToken);

        var categories = await categoryQuery
            .Skip((query.Page-1) * query.Size)
            .Take(query.Size)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            })
            .ToListAsync(cancellationToken);

        return new Response
        {
            Page = query.Page,
            TotalCount = totalCount,
            Categories = categories
        };
    }
}