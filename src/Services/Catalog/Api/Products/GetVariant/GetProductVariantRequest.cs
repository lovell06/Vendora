using Vendora.Services.Catalog.Application.Products;

namespace Vendora.Services.Catalog.Api.Products.GetVariant;

public sealed class GetProductVariantRequest
{
    public const string Pattern = "/variants/{productVariantId}";

    public required long ProductVariantId { get; init; }

    public GetProductVariant.Query ToQuery()
    {
        return new GetProductVariant.Query(ProductVariantId);
    }
}