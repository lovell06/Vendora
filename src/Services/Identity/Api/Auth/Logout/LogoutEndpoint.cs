using MediatR;
using Vendora.Services.Identity.Api.Extensions;

namespace Vendora.Services.Identity.Api.Auth.Logout;

public static class LogoutEndpoint
{
    public static void MapLogoutEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/logout", Handle);
    }

    public static async Task<IResult> Handle(
        LogoutRequest request, 
        CancellationToken cancellationToken, 
        ISender sender)
    {
        var command = request.ToCommand();

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok();
    }
}