using Vendora.Services.Catalog.Application.Products;

namespace Vendora.Services.Catalog.Api.Products.Reactivate;

public sealed class ReactivateProductRequest
{
    public const string Pattern = "/reactivate/{id}";

    public long Id { get; init; }

    public ReactivateProduct.Command ToCommand()
    {
        return new ReactivateProduct.Command(Id);
    }
}