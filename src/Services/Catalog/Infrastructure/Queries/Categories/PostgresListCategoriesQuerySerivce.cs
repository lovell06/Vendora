using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Application.Categories.List;
using Vendora.Services.Catalog.Infrastructure.Persistence;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Categories;

public sealed class PostgresListCategoriesQuerySerivce(PostgresDbContext context) : IListCategoryQueryService
{
    public async Task<Response> GetByPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var categories = await context.Categories
            .Skip((pageNumber-1) * pageSize)
            .Take(pageSize)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            })
            .ToListAsync(cancellationToken);

        return new Response { Categories = categories };
    }
}