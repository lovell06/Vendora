using Vendora.Services.Cart.Application.Carts;

namespace Vendora.Services.Cart.Api.Carts.RemoveItem;

public static class RemoveCartItemEndpoint
{
    extension(RouteGroupBuilder carts)
    {
        public void MapRemoveCartItemEndpoint()
        {
            carts.MapDelete(RemoveCartItemRequest.Pattern, Handle)
                .RequireAuthorization();
        }
    }

    private static async Task<IResult> Handle(
        [AsParameters] RemoveCartItemRequest request,
        ICurrentUser user,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var cmd = new RemoveCartItem.Command(user.UserId, request.ProductVariantId);

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.NoContent();
    }
}