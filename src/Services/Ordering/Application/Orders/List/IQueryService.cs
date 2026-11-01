namespace Vendora.Services.Ordering.Application.Orders.List;

public interface IQueryService
{
    Task<Response> ExecuteAsync(Query query, CancellationToken ct);
}