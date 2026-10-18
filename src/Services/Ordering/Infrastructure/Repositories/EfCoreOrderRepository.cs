namespace Vendora.Services.Ordering.Infrastructure.Repositories;

public sealed class EfCoreOrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public void Add(Order order)
    {
        context.Add(order);
    }

    public async Task<Order?> GetAsync(long orderId, CancellationToken ct)
    {
        return await context.Orders
            .SingleOrDefaultAsync(o => o.Id == orderId, ct);
    }

    public async Task<Order?> GetWithItemAsync(long orderId, CancellationToken ct)
    {
        return await context.Orders
            .Include(o => o.Items)
            .SingleOrDefaultAsync(o => o.Id == orderId, ct);
    }

    public async Task<List<Order>> ListAsync(CancellationToken ct)
    {
        return await context.Orders.ToListAsync(ct);
    }

    public async Task<List<Order>> ListWithItemAsync(CancellationToken ct)
    {
        return await context.Orders
            .Include(o => o.Items)
            .ToListAsync(ct);
    }
}