using MediatR;
using Vendora.Services.Identity.Api.Extensions;

namespace Vendora.Services.Identity.Api.Features.Auth.Login;

public static class LoginEndpoint
{
    public static void MapLoginEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/login", Handle);
    }

    private static async Task<IResult> Handle(
        LoginRequest request, 
        CancellationToken cancellationToken,
        ISender sender)
    {
        var command = request.ToCommand();

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok(result.Value);
    }
}