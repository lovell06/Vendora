using MediatR;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Categories.Restore;

public static class RestoreCategoryEndpoint
{
    public static void MapRestoreCategoryEndpoint(this RouteGroupBuilder group)
    {
        group.MapPatch(RestoreCategoryRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        [AsParameters] RestoreCategoryRequest request,
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