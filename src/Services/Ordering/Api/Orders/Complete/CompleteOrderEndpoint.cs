using Vendora.Services.Ordering.Api.Constants;

namespace Vendora.Services.Ordering.Api.Orders.Complete;

public static class CompleteOrderEndpoint
{
    extension(RouteGroupBuilder group)
    {
        public void MapCompleteOrderEndpoint()
        {
            group.MapPost(CompleteOrderRequest.Pattern, Handle)
                .RequireAuthorization(AuthorizationPolicies.ManageOrderLifecycle);
        }
    }

    private static async Task<IResult> Handle(
        CompleteOrderRequest request,
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