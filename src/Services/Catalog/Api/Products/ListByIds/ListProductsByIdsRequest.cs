using Vendora.Services.Catalog.Application.Products;

namespace Vendora.Services.Catalog.Api.Products.ListByIds;

public sealed class ListProductsByIdsRequest
{
    public const string Pattern = "/batch";

    public required List<long> ProductVariantIds { get; init; }

    public ListProductsByIds.Query ToQuery()
    {
        return new ListProductsByIds.Query(ProductVariantIds);
    }
}