using Vendora.Services.Ordering.Api.Orders.Place;

namespace Vendora.Services.Ordering.Api.Orders;

public static class GroupMapper
{
    extension(IEndpointRouteBuilder api)
    {
        public void MapOrderingEndpoints()
        {
            var orders = api.MapGroup("/orders").WithTags("Orders");
            orders.MapPlaceOrderEndpoint();
        }
    }
}