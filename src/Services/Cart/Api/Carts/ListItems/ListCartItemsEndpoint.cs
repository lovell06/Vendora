using Vendora.Services.Cart.Application.Carts;

namespace Vendora.Services.Cart.Api.Carts.ListItems;

public static class ListCartItemsEndpoint
{
    extension(RouteGroupBuilder carts)
    {
        public void MapListCartItemsEndpoint()
        {
            carts.MapGet(ListCartItemsRequest.Pattern, Handle)
                .RequireAuthorization();
        }
    }

    private static async Task<IResult> Handle(
        ICurrentUser user,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new ListCartItems.Query(user.UserId);

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok(result.Value);
    }
}
