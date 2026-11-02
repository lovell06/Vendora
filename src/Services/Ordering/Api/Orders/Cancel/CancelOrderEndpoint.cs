namespace Vendora.Services.Ordering.Api.Orders.Cancel;

public static class CancelOrderEndpoint
{
    extension(RouteGroupBuilder group)
    {
        public void MapCancelOrderEndpoint()
        {
            group.MapPost(CancelOrderRequest.Pattern, Handle)
                .RequireAuthorization();
        }
    }

    private static async Task<IResult> Handle(
        CancelOrderRequest request,
        ISender sender,
        ICurrentUser currentUser,
        CancellationToken ct)
    {
        var cmd = request.ToCommand(currentUser.UserId);

        var result = await sender.Send(cmd, ct);
        
        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok();
    }
}