using MediatR;
using Vendora.Services.Inventory.Api.Extensions;

namespace Vendora.Services.Inventory.Api.InventoryItems.Create;

public static class CreateInventoryItemEndpoint
{
    public static void MapCreateInventoryItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost(CreateInventoryItemRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        CreateInventoryItemRequest request,
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