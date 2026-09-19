using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Application.Categories.Get;
using Vendora.Services.Catalog.Infrastructure.Persistence;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Categories;

public sealed class PostgresGetCategoryQueryService(PostgresDbContext context) : IGetCategoryQueryService
{
    public async  Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        return await context.Categories
            .Where(category => category.DeletedAt == null)
            .Select(category => new Response { Id = category.Id, Name = category.Name })
            .SingleOrDefaultAsync(category => category.Id == query.Id, cancellationToken);
    }
}