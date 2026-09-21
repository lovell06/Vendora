using Vendora.Services.Inventory.Application.Abstractions.Persistence;

namespace Vendora.Services.Inventory.Infrastructure.Persistence;

public sealed class PostgresUnitOfWork(PostgresDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}