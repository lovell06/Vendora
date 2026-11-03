using Vendora.Services.Ordering.Application.Orders.Prepare;

namespace Vendora.Services.Ordering.Api.Orders.Prepare;

public sealed class PrepareOrderRequest
{
    public const string Pattern = "/prepare";

    public long OrderId { get; init; }

    public Command ToCommand()
    {
        return new Command { OrderId = OrderId };
    }
}