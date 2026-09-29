using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using Vendora.Services.Cart.Api.Extensions;
using Vendora.Services.Cart.Application.Carts.Initialize;

namespace Vendora.Services.Cart.Api.Carts.Initialize;

public static class InitializeCartEndpoint
{
    public static void MapInitializeCartEndpoint(this RouteGroupBuilder carts)
    {
        carts.MapPost(InitializeCartRequest.Pattern, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        ISender sender,
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (sub is null)
        {
            var error = new StringBuilder();
            error.AppendLine($"Not found: {JwtRegisteredClaimNames.Sub}");
            error.AppendLine("System only contains: ");
            foreach (var claim in user.Claims)
            {
                error.AppendLine($"{nameof(claim.Type)}: {claim.Value};");
            }

            throw new InvalidOperationException(error.ToString());
        }

        var cmd = new Command { UserId = Guid.Parse(sub) };

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Created();
    }
}