namespace Vendora.Services.Ordering.Application.Orders.Get;

public interface IQueryService
{
    Task<Response?> GetOrderAsync(Query query, CancellationToken ct);
}