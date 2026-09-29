using MediatR;
using Vendora.Services.Cart.Api.Extensions;
using Vendora.Services.Cart.Application.Abstractions.Authentication;

namespace Vendora.Services.Cart.Api.Carts.AddItem;

public static class AddItemEndpoint
{
    public static void MapAddItemEndpoint(this RouteGroupBuilder carts)
    {
        carts.MapPost(AddItemRequest.Pattern, Handle)
            .RequireAuthorization();
    }

    public static async Task<IResult> Handle(
        AddItemRequest request,
        ISender sender,
        ICurrentUser user,
        CancellationToken cancellationToken)
    {
        var cmd = request.ToCommand(user.UserId);

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Created();
    }
}