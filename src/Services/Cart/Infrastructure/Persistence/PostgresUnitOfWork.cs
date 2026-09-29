using Vendora.Services.Cart.Application.Abstractions.Persistence;

namespace Vendora.Services.Cart.Infrastructure.Persistence;

public sealed class PostgresUnitOfWork(PostgresDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await context.SaveChangesAsync(ct);
    }
}