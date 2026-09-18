using MediatR;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Products.Reactivate;

public static class ReactivateProductEndpoint
{
    public static void MapReactivateProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapPatch(ReactivateProductRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        [AsParameters] ReactivateProductRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var cmd = request.ToCommand();

        var result = await sender.Send(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.NoContent();
    }
}