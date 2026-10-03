namespace Vendora.Services.Cart.Api.Carts.Initialize;

public static class InitializeCartEndpoint
{
    public static void MapInitializeCartEndpoint(this RouteGroupBuilder carts)
    {
        carts.MapPost(InitializeCartRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        InitializeCartRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var cmd = request.ToCommand();

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Created();
    }
}
