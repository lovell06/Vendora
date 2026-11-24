using Vendora.Services.Catalog.Application.Products;

namespace Vendora.Services.Catalog.Api.Products.Discontinue;

public sealed class DiscontinueProductRequest
{
    public const string Pattern = "/discontinue/{id}";

    public long Id { get; init; }

    public DiscontinueProduct.Command ToCommand()
    {
        return new DiscontinueProduct.Command(Id);
    }
}