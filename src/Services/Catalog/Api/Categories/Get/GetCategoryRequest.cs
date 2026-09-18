using Vendora.Services.Catalog.Application.Categories.Get;

namespace Vendora.Services.Catalog.Api.Categories.Get;

public sealed class GetCategoryRequest
{
    public const string Pattern = "/{id}";

    public int Id { get; init; }

    public Query ToQuery()
    {
        return new Query
        {
            Id = Id
        };
    }
}