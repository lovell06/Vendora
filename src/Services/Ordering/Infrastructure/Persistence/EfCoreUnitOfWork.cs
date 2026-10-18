namespace Vendora.Services.Ordering.Infrastructure.Persistence;

public sealed class EfCoreUnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await context.SaveChangesAsync(ct);
    }
}
