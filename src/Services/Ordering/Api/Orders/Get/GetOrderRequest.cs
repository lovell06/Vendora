using Vendora.Services.Ordering.Application.Orders;

namespace Vendora.Services.Ordering.Api.Orders.Get;

public sealed class GetOrderRequest
{
    public const string Pattern = "/{orderId}";

    public long OrderId { get; init; }

    public GetOrder.Query ToQuery(Guid userId)
    {
        return new GetOrder.Query(userId, OrderId);
    }
}