using Vendora.Services.Cart.Application.Carts;

namespace Vendora.Services.Cart.Api.Carts.GetSelectedItems;

public static class GetSelectedCartItemsEndpoint
{
    extension(RouteGroupBuilder carts)
    {
        public void MapGetSelectedCartItemsEndpoint()
        {
            carts.MapPost(GetSelectedCartItemsRequest.Pattern, Handle)
                .RequireAuthorization();
        }
    }

    private static async Task<IResult> Handle(
        GetSelectedCartItemsRequest request,
        ISender sender,
        ICurrentUser user,
        CancellationToken ct)
    {
        var query = new GetSelectedCartItems.Query(
            user.UserId,
            [.. request.ProductVariantIds.Distinct()]);

        var result = await sender.Send(query, ct);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok(result.Value);
    }
}