using Vendora.Services.Ordering.Api.Constants;

namespace Vendora.Services.Ordering.Api.Orders.Prepare;

public static class PrepareOrderEndpoint
{
    extension(RouteGroupBuilder group)
    {
        public void MapPrepareOrderEndpoint()
        {
            group.MapPost(PrepareOrderRequest.Pattern, Handle)
                .RequireAuthorization(AuthorizationPolicies.ManageOrderLifecycle);
        }
    }

    private static async Task<IResult> Handle(
        PrepareOrderRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var cmd = request.ToCommand();

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.NoContent();
    }
}