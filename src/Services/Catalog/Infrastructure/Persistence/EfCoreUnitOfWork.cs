using Microsoft.EntityFrameworkCore.Storage;
using Vendora.Services.Catalog.Infrastructure.Persistence.Helpers;

namespace Vendora.Services.Catalog.Infrastructure.Persistence;

public sealed class EfCoreUnitOfWork(
    ApplicationDbContext context) : IUnitOfWork
{
    private static async Task PrepareForSaveChangesAsync(DbContext context, CancellationToken ct)
    {
        await ProductImageOrderHelper.PrepareAsync(context, ct);

        await PrimaryProductImageHelper.PrepareAsync(context, ct);

        await ProductVariantImageOrderHelper.PrepareAsync(context, ct);

        await PrimaryProductVariantImageHelper.PrepareAsync(context, ct);

    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        if (context.Database.CurrentTransaction is not null)
        {
            await PrepareForSaveChangesAsync(context, cancellationToken);

            return await context.SaveChangesAsync(cancellationToken);
        }

        await using var transaction = await BeginTransactionAsync(cancellationToken);

        await PrepareForSaveChangesAsync(context, cancellationToken);

        var affectedRow = await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        context.ChangeTracker.AcceptAllChanges();

        return affectedRow;
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