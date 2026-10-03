namespace Vendora.Services.Cart.Api.Carts.SetQuantity;

public static class SetQuantityEndpoint
{
    public static void MapSetQuantityEndpoint(this RouteGroupBuilder carts)
    {
        carts.MapPatch(SetQuantityRequest.Pattern, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        long productId,
        SetQuantityRequest request,
        ICurrentUser user,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var cmd = request.ToCommand(user.UserId, productId);

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.NoContent();
    }
}
