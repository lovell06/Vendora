using Vendora.Services.Cart.Application.Carts;

namespace Vendora.Services.Cart.Api.Carts.AddItem;

public static class AddCartItemEndpoint
{
    extension(RouteGroupBuilder carts)
    {
        public void MapAddCartItemEndpoint()
        {
            carts.MapPost(AddCartItemRequest.Pattern, Handle)
                .RequireAuthorization();
        }
    }

    private static async Task<IResult> Handle(
        AddCartItemRequest request,
        ISender sender,
        ICurrentUser user,
        CancellationToken cancellationToken)
    {
        var cmd = new AddCartItem.Command(
            user.UserId,
            request.ProductVariantId, 
            request.Quantity);

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Created();
    }
}