using Vendora.Services.Catalog.Application.Categories;

namespace Vendora.Services.Catalog.Api.Categories.List;

public sealed class ListCategoriesRequest
{
    public const string Pattern = "";

    public int Page { get; init; }
    public int Size { get; init; }

    public ListCategories.Query ToQuery()
    {
        return new ListCategories.Query(Page, Size);
    }
}
