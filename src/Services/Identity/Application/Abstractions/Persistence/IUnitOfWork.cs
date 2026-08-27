namespace Vendora.Services.Identity.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}