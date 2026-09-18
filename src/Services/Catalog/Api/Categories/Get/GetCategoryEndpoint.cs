using MediatR;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Categories.Get;

public static class GetCategoryEndpoint
{
    public static void MapGetCategoryEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet(GetCategoryRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        [AsParameters] GetCategoryRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok(result.Value);
    }
}