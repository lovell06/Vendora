namespace Vendora.Services.Catalog.Api.Products.ListByIds;

public static class ListProductsByIdsEndpoint
{
    extension(RouteGroupBuilder products)
    {
        public void MapListProductsByIdsEndpoint()
        {
            products.MapPost(ListProductsByIdsRequest.Pattern, Handle);
        }
    }
    
    private static async Task<IResult> Handle(
        ListProductsByIdsRequest request,
        ISender sender,
        CancellationToken ct)
    {
        var query = request.ToQuery();

        var result = await sender.Send(query, ct);

        if (result.IsFailure)
            return result.Error.ToHttpResult();
        
        return TypedResults.Ok(result.Value);
    }
}