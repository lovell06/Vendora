namespace Vendora.Services.Ordering.Application.Abstractions.Clients;

public interface IInventoryServiceClient
{
    Task<Result> ReserveAsync(IReadOnlyList<OrderItem> orderItems, CancellationToken ct);
}