using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Application.Categories.List;
using Vendora.Services.Catalog.Infrastructure.Persistence;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Categories;

public sealed class PostgresListCategoriesQuerySerivce(PostgresDbContext context) : IListCategoryQueryService
{
    public async Task<Response> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        var categories = await context.Categories
            .Where(category => category.DeletedAt == null)
            .Skip((query.Page-1) * query.Size)
            .Take(query.Size)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            })
            .ToListAsync(cancellationToken);

        return new Response { Categories = categories };
    }
}