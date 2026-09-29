using MediatR;
using Vendora.Services.Cart.Api.Extensions;
using Vendora.Services.Cart.Application.Abstractions.Authentication;
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
        ICurrentUser user,
        CancellationToken cancellationToken)
    {
        var cmd = new Command { UserId = user.UserId };

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Created();
    }
}