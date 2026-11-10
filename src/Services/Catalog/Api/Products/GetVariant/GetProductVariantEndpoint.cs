namespace Vendora.Services.Catalog.Api.Products.GetVariant;

public static class GetProductVariantEndpoint
{
    extension(RouteGroupBuilder group)
    {
        public void MapGetProductVariantEndpoint()
        {
            group.MapGet(GetProductVariantRequest.Pattern, Handle);
        }
    }

    private static async Task<IResult> Handle(
        [AsParameters] GetProductVariantRequest request,
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