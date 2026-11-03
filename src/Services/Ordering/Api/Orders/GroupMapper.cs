using Vendora.Services.Ordering.Api.Orders.Cancel;
using Vendora.Services.Ordering.Api.Orders.Deliver;
using Vendora.Services.Ordering.Api.Orders.Get;
using Vendora.Services.Ordering.Api.Orders.List;
using Vendora.Services.Ordering.Api.Orders.Place;
using Vendora.Services.Ordering.Api.Orders.Prepare;

namespace Vendora.Services.Ordering.Api.Orders;

public static class GroupMapper
{
    extension(IEndpointRouteBuilder api)
    {
        public void MapOrderingEndpoints()
        {
            var orders = api.MapGroup("/orders").WithTags("Orders");
            orders.MapPlaceOrderEndpoint();
            orders.MapGetOrderEndpoint();
            orders.MapListOrdersEndpoint();
            orders.MapCancelOrderEndpoint();
            orders.MapPrepareOrderEndpoint();
            orders.MapDeliverOrderEndpoint();
        }
    }
}