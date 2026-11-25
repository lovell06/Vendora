using Vendora.Services.Identity.Application.Abstractions.Authentication;
using Vendora.Services.Identity.Application.Authentication;

namespace Vendora.Services.Identity.Api.Auth.Logout;

public static class LogoutEndpoint
{
    public static void MapLogoutEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost(LogoutRequest.Pattern, Handle)
            .RequireAuthorization();
    }

    public static async Task<IResult> Handle(
        LogoutRequest request, 
        ICurrentUser user,
        CancellationToken cancellationToken, 
        ISender sender)
    {
        var command = new LogoutUser.Command(user.UserId, request.RefreshToken);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok();
    }
}