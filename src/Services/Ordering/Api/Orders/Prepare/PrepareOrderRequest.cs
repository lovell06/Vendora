using Vendora.Services.Ordering.Application.Orders;

namespace Vendora.Services.Ordering.Api.Orders.Prepare;

public sealed class PrepareOrderRequest
{
    public const string Pattern = "/prepare";

    public long OrderId { get; init; }

    public PrepareOrder.Command ToCommand()
    {
        return new PrepareOrder.Command(OrderId);
    }
}