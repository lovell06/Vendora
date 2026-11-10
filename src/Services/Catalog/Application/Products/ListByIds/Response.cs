namespace Vendora.Services.Catalog.Application.Products.ListByIds;

public class Response
{
    public required List<ProductVariantDto> ProductVariants { get; init; }
}

public sealed class ProductVariantDto
{
    public required long ProductId { get; init; }
    public required long VariantId { get; init; }
    public required string ProductName { get; init; }
    public required string VariantName { get; init; }
    public required string Sku { get; init; }
    public decimal Price { get; init; }
    public required string Currency { get; init; }
}
