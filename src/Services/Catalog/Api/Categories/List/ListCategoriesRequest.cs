using Vendora.Services.Catalog.Application.Categories.List;

namespace Vendora.Services.Catalog.Api.Categories.List;

public sealed class ListCategoriesRequest
{
    public const string Pattern = "";

    public int Page { get; init; }
    public int Size { get; init; }

    public Query ToQuery()
    {
        return new Query
        {
            Page = Page,
            Size = Size
        };
    }
}