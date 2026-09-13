using Vendora.Services.Catalog.Application.Abstractions.Persistence;

namespace Vendora.Services.Catalog.Infrastructure.Persistence.UnitOfWork;

public class PostgresUnitOfWork(PostgresDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}