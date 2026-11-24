using Vendora.Services.Catalog.Application.Products;

namespace Vendora.Services.Catalog.Api.Products.List;

public sealed class ListProductsRequest
{
    public const string Pattern = "";
    public int? CategoryId { get; init; }
    public int Page { get; init; }
    public int Size { get; init; }
    public string? Search { get; init; }

    public ListProducts.Query ToQuery()
    {
        return new ListProducts.Query(CategoryId, Page, Size, Search);
    }
}
