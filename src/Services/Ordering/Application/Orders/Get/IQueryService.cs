namespace Vendora.Services.Ordering.Application.Orders.Get;

public interface IQueryService
{
    Task<Response?> ExecuteAsync(Query query, CancellationToken ct);
}