using MediatR;
using Vendora.Services.Inventory.Api.Extensions;

namespace Vendora.Services.Inventory.Api.InventoryItems.CheckAvailability;

public static class CheckAvailabilityEndpoint
{
    public static void MapCheckAvailabilityEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet(CheckAvailabilityRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        [AsParameters] CheckAvailabilityRequest request,
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