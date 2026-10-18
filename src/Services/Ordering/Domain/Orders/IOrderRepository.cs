namespace Vendora.Services.Ordering.Domain.Orders;

public interface IOrderRepository
{
    void Add(Order order);
    Task<Order?> GetAsync(long orderId, CancellationToken ct);
    Task<Order?> GetWithItemAsync(long orderId, CancellationToken ct);
    Task<List<Order>> ListAsync(CancellationToken ct);
    Task<List<Order>> ListWithItemAsync(CancellationToken ct);
}