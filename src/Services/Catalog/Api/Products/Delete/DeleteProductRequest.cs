using Vendora.Services.Catalog.Application.Products;

namespace Vendora.Services.Catalog.Api.Products.Delete;

public sealed class DeleteProductRequest
{
    public const string Pattern = "/delete/{id}";

    public long Id { get; init; }

    public DeleteProduct.Command ToCommand()
    {
        return new DeleteProduct.Command(Id);
    }
}