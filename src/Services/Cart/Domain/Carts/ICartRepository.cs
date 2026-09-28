namespace Vendora.Services.Cart.Domain.Carts;

public interface ICartRepository
{
    Task<bool> ExistsByUserId(Guid userId, CancellationToken ct);
    Task<Cart?> GetAsync(Guid userId, CancellationToken ct);
    Task<Cart?> GetWithItemsAsync(Guid userId, CancellationToken ct);
    Task<ICollection<Cart>> ListAsync(CancellationToken ct);
    void Add(Cart cart);
}