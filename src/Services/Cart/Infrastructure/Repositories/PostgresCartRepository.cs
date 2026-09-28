using Microsoft.EntityFrameworkCore;
using Vendora.Services.Cart.Domain.Carts;
using Vendora.Services.Cart.Infrastructure.Persistence;

namespace Vendora.Services.Cart.Infrastructure.Repositories;

using CartAggregate = Domain.Carts.Cart;

public sealed class PostgresCartRepository(PostgresDbContext context) : ICartRepository
{
    public void Add(CartAggregate cart)
    {
        context.Carts.Add(cart);
    }

    public async Task<bool> ExistsByUserId(Guid userId, CancellationToken ct)
    {
        return await context.Carts
            .SingleOrDefaultAsync(cart => cart.UserId == userId, ct) is not null;
    }

    public async Task<CartAggregate?> GetAsync(Guid userId, CancellationToken ct)
    {
        return await context.Carts
            .SingleOrDefaultAsync(cart => cart.UserId == userId, ct);
    }

    public async Task<CartAggregate?> GetWithItemsAsync(Guid userId, CancellationToken ct)
    {
        return await context.Carts
            .Include(cart => cart.Items)
            .SingleOrDefaultAsync(cart => cart.UserId == userId, ct);
    }

    public async Task<ICollection<CartAggregate>> ListAsync(CancellationToken ct)
    {
        return await context.Carts.ToListAsync(ct);
    }
}