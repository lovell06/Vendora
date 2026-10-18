using Vendora.Services.Catalog.Application.Products.ListByIds;

namespace Vendora.Services.Catalog.Api.Products.ListByIds;

public sealed class ListProductsByIdsRequest
{
    public const string Pattern = "/batch";

    public required List<long> ProductIds { get; init; }

    public Query ToQuery()
    {
        return new Query { ProductIds = ProductIds };
    }
}