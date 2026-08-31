using Microsoft.EntityFrameworkCore;
using Vendora.Services.Identity.Domain.Users;

namespace Vendora.Services.Identity.Infrastructure.Persistence.Repositories;

public class PostgresUserRepository(PostgresDbContext context) : IUserRepository
{
    public void Add(User user)
    {
        context.Users.Add(user);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        var user = await context.Users.SingleOrDefaultAsync(u => u.Email == email, ct);

        return user is not null;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Id == id, ct);
    }
}