using Vendora.Services.Ordering.Application.Orders;

namespace Vendora.Services.Ordering.Api.Orders.Complete;

public sealed class CompleteOrderRequest
{
    public const string Pattern = "/complete";

    public long OrderId { get; init; }

    public CompleteOrder.Command ToCommand()
    {
        return new CompleteOrder.Command(OrderId);
    }
}