namespace Vendora.Services.Cart.Infrastructure.Persistence;

public sealed class EfCoreUnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await context.SaveChangesAsync(ct);
    }
}