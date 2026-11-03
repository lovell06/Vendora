using Vendora.Services.Ordering.Api.Constants;

namespace Vendora.Services.Ordering.Api.Orders.Deliver;

public static class DeliverOrderEndpoint
{
    extension(RouteGroupBuilder group)
    {
        public void MapDeliverOrderEndpoint()
        {
            group.MapPost(DeliverOrderRequest.Pattern, Handle)
                .RequireAuthorization(AuthorizationPolicies.ManageOrderLifecycle);
        }
    }

    private static async Task<IResult> Handle(
        DeliverOrderRequest request,
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