using MediatR;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Products.Restore;

public static class RestoreProductEndpoint
{
    public static void MapRestoreProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapPatch(RestoreProductRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        [AsParameters] RestoreProductRequest request,
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