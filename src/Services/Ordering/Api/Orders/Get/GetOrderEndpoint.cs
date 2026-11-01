namespace Vendora.Services.Ordering.Api.Orders.Get;

public static class GetOrderEndpoint
{
    extension(RouteGroupBuilder group)
    {
        public void MapGetOrderEndpoint()
        {
            group.MapGet(GetOrderRequest.Pattern, Handle)
                .RequireAuthorization();
        }
    }

    private static async Task<IResult> Handle(
        [AsParameters] GetOrderRequest request,
        ISender sender,
        ICurrentUser currentUser,
        CancellationToken ct)
    {
        var query = request.ToQuery(currentUser.UserId);

        var result = await sender.Send(query, ct);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok(result.Value);
    }
}