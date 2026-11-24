using Vendora.Services.Catalog.Application.Categories;

namespace Vendora.Services.Catalog.Api.Categories.Get;

public sealed class GetCategoryRequest
{
    public const string Pattern = "/{id}";

    public int Id { get; init; }

    public GetCategory.Query ToQuery()
    {
        return new GetCategory.Query(Id);
    }
}
