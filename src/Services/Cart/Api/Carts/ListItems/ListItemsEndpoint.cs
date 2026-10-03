using Vendora.Services.Cart.Application.Carts.ListItems;

namespace Vendora.Services.Cart.Api.Carts.ListItems;

public static class ListItemsEndpoint
{
    public static void MapListItemsEndpoint(this RouteGroupBuilder carts)
    {
        carts.MapGet(ListItemsRequest.Pattern, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        ICurrentUser user,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new Query { UserId = user.UserId };

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok(result.Value);
    }
}
