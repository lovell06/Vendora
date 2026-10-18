namespace Vendora.Services.Ordering.Api.Orders.Place;

public static class PlaceOrderEndpoint
{
    extension(RouteGroupBuilder orders)
    {
        public void MapPlaceOrderEndpoint()
        {
            orders.MapPost(PlaceOrderRequest.Pattern, Handle)
                .RequireAuthorization();
        }
    }

    private static async Task<IResult> Handle(
        PlaceOrderRequest request,
        ISender sender,
        ICurrentUser user,
        CancellationToken ct)
    {
        var cmd = request.ToCommand(user.UserId);

        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            return result.Error.ToHttpResult();
        
        return TypedResults.Created();
    }
}