using Vendora.Services.Catalog.Application.Products;

namespace Vendora.Services.Catalog.Api.Products.Restore;

public class RestoreProductRequest
{
    public const string Pattern = "/restore/{id}";

    public long Id { get; init; }

    public RestoreProduct.Command ToCommand()
    {
        return new RestoreProduct.Command(Id);
    }
}