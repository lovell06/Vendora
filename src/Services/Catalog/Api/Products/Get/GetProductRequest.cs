using Vendora.Services.Catalog.Application.Products.Get;

namespace Vendora.Services.Catalog.Api.Products.Get;

public class GetProductRequest
{
    public const string Pattern = "/{id}";
    public long Id { get; init; }

    public Query ToQuery()
    {
        return new Query { Id = Id };
    }
}