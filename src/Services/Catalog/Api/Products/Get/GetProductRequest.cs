using Vendora.Services.Catalog.Application.Products;

namespace Vendora.Services.Catalog.Api.Products.Get;

public class GetProductRequest
{
    public const string Pattern = "/{id}";
    public long Id { get; init; }

    public GetProduct.Query ToQuery()
    {
        return new GetProduct.Query(Id);
    }
}
