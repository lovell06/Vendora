using Vendora.Services.Ordering.Application.Orders.Deliver;

namespace Vendora.Services.Ordering.Api.Orders.Deliver;

public sealed class DeliverOrderRequest
{
    public const string Pattern = "/deliver";

    public long OrderId { get; init; }

    public Command ToCommand()
    {
        return new Command { OrderId = OrderId };
    }
}