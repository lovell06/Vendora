using MediatR;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Products.List;

public static class ListProductsEndpoint
{
    public static void MapListProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/products", Handle);
    }

    public static async Task<IResult> Handle([AsParameters] ListProductsRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToHttpResult();

        return TypedResults.Ok(result.Value);
    }
}