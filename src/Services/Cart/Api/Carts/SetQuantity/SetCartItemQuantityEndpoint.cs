using Vendora.Services.Cart.Application.Carts;

namespace Vendora.Services.Cart.Api.Carts.SetQuantity;

public static class SetCartItemQuantityEndpoint
{
    extension(RouteGroupBuilder carts)
    {
        public void MapSetCartItemQuantityEndpoint()
        {
            carts.MapPatch(SetCartItemQuantityRequest.Pattern, Handle)
                .RequireAuthorization();
        }
    }

    private static async Task<IResult> Handle(
        long productVariantId,
        SetCartItemQuantityRequest request,
        ICurrentUser user,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var cmd = new SetCartItemQuantity.Command(
            user.UserId, 
            productVariantId, 
            request.Quantity);

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.NoContent();
    }
}
