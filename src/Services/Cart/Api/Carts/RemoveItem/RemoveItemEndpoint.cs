namespace Vendora.Services.Cart.Api.Carts.RemoveItem;

public static class RemoveItemEndpoint
{
    public static void MapRemoveItemEndpoint(this RouteGroupBuilder carts)
    {
        carts.MapDelete(RemoveItemRequest.Pattern, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [AsParameters] RemoveItemRequest request,
        ICurrentUser user,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var cmd = request.ToCommand(user.UserId);

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.NoContent();
    }
}