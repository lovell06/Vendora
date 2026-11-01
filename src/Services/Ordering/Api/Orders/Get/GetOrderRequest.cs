using Vendora.Services.Ordering.Application.Orders.Get;

namespace Vendora.Services.Ordering.Api.Orders.Get;

public sealed class GetOrderRequest
{
    public const string Pattern = "/{orderId}";

    public long OrderId { get; init; }

    public Query ToQuery(Guid userId)
    {
        return new Query { UserId = userId, OrderId = OrderId };
    }
}