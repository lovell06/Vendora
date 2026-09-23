using MediatR;
using Vendora.Services.Inventory.Api.Constants;
using Vendora.Services.Inventory.Api.Extensions;

namespace Vendora.Services.Inventory.Api.InventoryItems.List;

public static class ListInventoryItemsEndpoint
{
    public static void MapListInventoryItemsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet(ListInventoryItemsRequest.Pattern, Handle)
            .RequireAuthorization(AuthorizationPolicies.ManageStock);
    }

    private static async Task<IResult> Handle(
        [AsParameters] ListInventoryItemsRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok(result.Value);
    }
}