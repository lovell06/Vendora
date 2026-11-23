namespace Vendora.Services.Catalog.Application.Products.Get;

public sealed class Response
{
    public required ProductDto Product { get; init; }
}

public sealed class ProductDto
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; } = null;
    public required string Brand { get; init; }
    public required CategoryDto Category { get; init; }
    public required string Currency { get; init; }
    public required string Status { get; init; }
    public required IReadOnlyList<ProductVariantDto> Variants { get; init; }
    public required IReadOnlyList<ProductImageDto> Images { get; init; }
}

public sealed class ProductImageDto
{
    public required long Id { get; init; }
    public required string Url { get; init; }
    public required int SortOrder { get; init; }
}

public sealed class ProductVariantDto
{
    public required string Sku { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public required IReadOnlyList<long> ImageIds { get; init; }
}

public sealed class CategoryDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
}