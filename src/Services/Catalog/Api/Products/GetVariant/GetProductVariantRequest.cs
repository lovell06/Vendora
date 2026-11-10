using Vendora.Services.Catalog.Application.Products.GetVariant;

namespace Vendora.Services.Catalog.Api.Products.GetVariant;

public sealed class GetProductVariantRequest
{
    public const string Pattern = "/variants/{productVariantId}";

    public required long ProductVariantId { get; init; }

    public Query ToQuery()
    {
        return new Query { ProductVariantId = ProductVariantId };
    }
}