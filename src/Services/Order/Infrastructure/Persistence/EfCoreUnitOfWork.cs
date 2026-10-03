namespace Vendora.Services.Order.Infrastructure.Persistence;

public sealed class EfCoreUnitOfWork(PostgresDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await context.SaveChangesAsync(ct);
    }
}
