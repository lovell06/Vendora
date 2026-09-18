using MediatR;
using Vendora.Services.Catalog.Api.Extensions;

namespace Vendora.Services.Catalog.Api.Products.Get;

public static class GetProductEndpoint
{
    public static void MapGetProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet(GetProductRequest.Pattern, Handle);
    }

    private static async Task<IResult> Handle(
        [AsParameters] GetProductRequest request,
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