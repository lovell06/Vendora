using Vendora.Services.Ordering.Application.Orders;

namespace Vendora.Services.Ordering.Api.Orders.Cancel;

public sealed class CancelOrderRequest
{
    public const string Pattern = "/cancel";

    public long OrderId { get; init; }

    public CancelOrder.Command ToCommand(Guid userId)
    {
        return new CancelOrder.Command(userId, OrderId);
    }
}