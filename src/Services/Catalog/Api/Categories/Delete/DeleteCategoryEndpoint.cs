using MediatR;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Categories.Delete;

public static class DeleteCategoryEndpoint
{
    public static void MapDeleteCategoryEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete(DeleteCategoryRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        [AsParameters] DeleteCategoryRequest request,
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