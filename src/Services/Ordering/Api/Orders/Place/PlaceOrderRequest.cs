using Vendora.Services.Ordering.Application.Orders;

namespace Vendora.Services.Ordering.Api.Orders.Place;

public sealed class PlaceOrderRequest
{
    public const string Pattern = "/place";

    public required IReadOnlyList<long> ProductVariantIds { get; init; }

    public PlaceOrder.Command ToCommand(Guid userId)
    {
        return new PlaceOrder.Command(
            userId,
            ProductVariantIds);
    }
}