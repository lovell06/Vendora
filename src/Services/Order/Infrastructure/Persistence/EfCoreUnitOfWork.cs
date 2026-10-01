using Microsoft.EntityFrameworkCore;
using Vendora.Services.Order.Application.Abstractions.Persistence;

namespace Vendora.Services.Order.Infrastructure.Persistence;

public sealed class EfCoreUnitOfWork(DbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await context.SaveChangesAsync(ct);
    }
}