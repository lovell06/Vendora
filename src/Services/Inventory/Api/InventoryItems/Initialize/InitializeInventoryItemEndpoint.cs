using MediatR;
using Vendora.Services.Inventory.Api.Constants;
using Vendora.Services.Inventory.Api.Extensions;

namespace Vendora.Services.Inventory.Api.InventoryItems.Initialize;

public static class InitializeInventoryItemEndpoint
{
    public static void MapCreateInventoryItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost(InitializeInventoryItemRequest.Pattern, Handle)
            .RequireAuthorization(AuthorizationPolicies.ManageStock);
    }

    private static async Task<IResult> Handle(
        InitializeInventoryItemRequest request,
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