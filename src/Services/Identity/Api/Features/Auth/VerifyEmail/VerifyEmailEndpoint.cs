using MediatR;
using Vendora.Services.Identity.Api.Extensions;

namespace Vendora.Services.Identity.Api.Features.Auth.VerifyEmail;

public static class VerifyEmailEndpoint
{
    public static void MapVerifyEmailEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/verify-email", Handle);
    }

    public static async Task<IResult> Handle(
        VerifyEmailRequest request, 
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await sender.Send(
            request: command,
            cancellationToken: cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : result.Error.ToProblem();
    }
}