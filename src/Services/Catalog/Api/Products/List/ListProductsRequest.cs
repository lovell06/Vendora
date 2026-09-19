using Vendora.Services.Catalog.Application.Products.List;

namespace Vendora.Services.Catalog.Api.Products.List;

public sealed class ListProductsRequest
{
    public const string Pattern = "";
    public int? CategoryId { get; init; }
    public int Page { get; init; }
    public int Size { get; init; }

    public Query ToQuery()
    {
        return new Query
        {
            CategoryId = CategoryId,
            Page = Page,
            Size = Size
        };
    }
}