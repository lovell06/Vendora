using Vendora.Services.Ordering.Application.Orders.Place;

namespace Vendora.Services.Ordering.Api.Orders.Place;

public sealed class PlaceOrderRequest
{
    public const string Pattern = "/place";

    public required IReadOnlyList<long> ProductIds { get; init; }

    public Command ToCommand(Guid userId)
    {
        return new Command
        {
            UserId = userId,
            ProductIds = ProductIds
        };
    }
}