using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using Vendora.Services.Inventory.Api.Constants;
using Vendora.Services.Inventory.Api.Extensions;

namespace Vendora.Services.Inventory.Api.InventoryItems.Adjust;

public static class AdjustInventoryItemEndpoint
{
    public static void MapAdjustInventoryItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost(AdjustInventoryItemRequest.Pattern, Handle)
            .RequireAuthorization(AuthorizationPolicies.ManageStock);
    }

    private static async Task<IResult> Handle(
        AdjustInventoryItemRequest request,
        ISender sender,
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (sub is null)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Not found: \"{JwtRegisteredClaimNames.Sub}\";");
            stringBuilder.AppendLine("System onsly contains:");
            foreach(var claim in user.Claims)
            {
                stringBuilder.AppendLine($"{nameof(claim.Type)}: \"{claim.Type}\";");
            }

            throw new InvalidOperationException(stringBuilder.ToString());
        }
        
        var userId = Guid.Parse(sub);

        var command = request.ToCommand(userId);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.NoContent();
    }
}