using Vendora.Services.Ordering.Application.Orders.Complete;

namespace Vendora.Services.Ordering.Api.Orders.Complete;

public sealed class CompleteOrderRequest
{
    public const string Pattern = "/complete";

    public long OrderId { get; init; }

    public Command ToCommand()
    {
        return new Command { OrderId = OrderId };
    }
}