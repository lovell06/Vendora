using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Domain.Categories;

namespace Vendora.Services.Catalog.Infrastructure.Persistence.Repositories;

public class PostgresCategoryRepository(PostgresDbContext context) : ICategoryRepository
{
    public async Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Categories
            .SingleOrDefaultAsync(cat => cat.Id == id, cancellationToken) is not null;
    }

    public void Add(Category category)
    {
        context.Add(category);
    }

    public async Task<ICollection<Category>> GetAsync(CancellationToken cancellationToken)
    {
        return await context.Categories.ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Categories.SingleOrDefaultAsync(cat => cat.Id == id, cancellationToken);
    }
}