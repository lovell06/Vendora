namespace Vendora.Services.Ordering.Api.Orders.List;

public static class ListOrdersEndpoint
{
    extension(RouteGroupBuilder group)
    {
        public void MapListOrdersEndpoint()
        {
            group.MapGet(ListOrdersRequest.Pattern, Handle)
                .RequireAuthorization();
        }
    }

    private static async Task<IResult> Handle(
        [AsParameters] ListOrdersRequest request,
        ISender sender,
        ICurrentUser currentUser,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery(currentUser.UserId);

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok(result.Value);
    }
}