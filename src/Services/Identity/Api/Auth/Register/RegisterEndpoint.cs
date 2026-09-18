using MediatR;
using Vendora.Services.Identity.Api.Extensions;

namespace Vendora.Services.Identity.Api.Auth.Register;

public static class RegisterEndpoint
{
    public static void MapRegisterEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost(RegisterRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        RegisterRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await sender.Send(
            request: command,
            cancellationToken: cancellationToken);

        return result.IsSuccess ? TypedResults.Created() : result.Error.ToHttpResult();
    }
}