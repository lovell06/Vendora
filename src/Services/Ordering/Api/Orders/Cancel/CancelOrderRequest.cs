using Vendora.Services.Ordering.Application.Orders.Cancel;

namespace Vendora.Services.Ordering.Api.Orders.Cancel;

public sealed class CancelOrderRequest
{
    public const string Pattern = "/cancel";

    public long OrderId { get; init; }

    public Command ToCommand(Guid userId)
    {
        return new Command { UserId = userId, OrderId = OrderId };
    }
}