using Microsoft.EntityFrameworkCore.Storage;

namespace Vendora.Services.Catalog.Infrastructure.Persistence;

public sealed class EfCoreUnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        return new EfCoreUnitOfWorkTransaction(transaction);
    }
}

public sealed class EfCoreUnitOfWorkTransaction(IDbContextTransaction transaction) : IUnitOfWorkTransaction
{
    public async ValueTask DisposeAsync()
    {
        await transaction.DisposeAsync();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await transaction.RollbackAsync(cancellationToken);
    }
}