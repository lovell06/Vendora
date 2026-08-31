using Vendora.Services.Identity.Application.Abstractions.Persistence;

namespace Vendora.Services.Identity.Infrastructure.Persistence.UnitOfWork;

public class PostgresUnitOfWork(PostgresDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}