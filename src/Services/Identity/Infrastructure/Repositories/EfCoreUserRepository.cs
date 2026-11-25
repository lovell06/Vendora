namespace Vendora.Services.Identity.Infrastructure.Repositories;

public class EfCoreUserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<bool> IsEmailVerification(string email, CancellationToken ct)
    {
        return await context.Users.AnyAsync(user
            => user.Email == email && user.EmailVerifiedAt != null, ct);
    }

    public void Add(User user)
    {
        context.Users.Add(user);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        return await context.Users.AnyAsync(u => u.Email == email, ct);
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