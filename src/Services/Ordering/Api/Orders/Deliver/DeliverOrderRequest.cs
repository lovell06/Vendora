using Vendora.Services.Ordering.Application.Orders;

namespace Vendora.Services.Ordering.Api.Orders.Deliver;

public sealed class DeliverOrderRequest
{
    public const string Pattern = "/deliver";

    public long OrderId { get; init; }

    public DeliverOrder.Command ToCommand()
    {
        return new DeliverOrder.Command(OrderId);
    }
}