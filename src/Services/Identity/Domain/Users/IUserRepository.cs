namespace Vendora.Services.Identity.Domain.Users;

public interface IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
    public void Add(User user);
}