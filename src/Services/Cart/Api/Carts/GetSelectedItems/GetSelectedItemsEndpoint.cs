namespace Vendora.Services.Cart.Api.Carts.GetSelectedItems;

public static class GetSelectedItemsEndpoint
{
    extension(RouteGroupBuilder carts)
    {
        public void MapGetSelectedItemsEndpoint()
        {
            carts.MapPost(GetSelectedItemsRequest.Pattern, Handle)
                .RequireAuthorization();
        }
    }

    private static async Task<IResult> Handle(
        GetSelectedItemsRequest request,
        ISender sender,
        ICurrentUser user,
        CancellationToken ct)
    {
        var query = request.ToQuery(user.UserId);

        var result = await sender.Send(query, ct);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok(result.Value);
    }
}