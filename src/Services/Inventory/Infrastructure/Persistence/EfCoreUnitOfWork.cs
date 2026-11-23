using Vendora.Services.Inventory.Application.Abstractions.Persistence.Exceptions;

namespace Vendora.Services.Inventory.Infrastructure.Persistence;

public sealed class EfCoreUnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException e)
        {
            context.ChangeTracker.Clear();
            throw new ConcurrencyConflictException("Inventory item was modified concurrently.", e);
        }
    }
}