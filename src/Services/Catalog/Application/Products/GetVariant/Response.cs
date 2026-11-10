namespace Vendora.Services.Catalog.Application.Products.GetVariant;

public sealed class Response
{
    public required ProductVariantDto ProductVariant { get; init; }
}

public sealed class ProductVariantDto
{
    public required long ProductId { get; init; }
    public required long ProductVariantId { get; init; }
    public required string Sku { get; init; }
}