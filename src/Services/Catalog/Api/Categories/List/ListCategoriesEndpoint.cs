using MediatR;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Categories.List;

public static class ListCategoriesEndpoint
{
    public static void MapListCategoriesEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet(ListCategoriesRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        [AsParameters] ListCategoriesRequest request,
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