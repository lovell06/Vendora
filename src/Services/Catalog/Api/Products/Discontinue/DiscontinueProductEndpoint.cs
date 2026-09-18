using MediatR;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Products.Discontinue;

public static class DiscontinueProductEndpoint
{
    public static void MapDiscontinueProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapPatch(DiscontinueProductRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        [AsParameters] DiscontinueProductRequest request,
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