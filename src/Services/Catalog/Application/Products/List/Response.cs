namespace Vendora.Services.Catalog.Application.Products.List;

public sealed class Response
{
    public CategoryDto? Category { get; init; }
    public required IReadOnlyList<ProductDto> Products { get; init; }
}

public sealed class CategoryDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
}

public sealed class ProductDto
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public decimal Price { get; init; }
    public required string Currency { get; init; }
}