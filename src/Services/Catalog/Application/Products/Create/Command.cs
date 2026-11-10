namespace Vendora.Services.Catalog.Application.Products.Create;

public sealed class Command : ICommand
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string Brand { get; init; }
    public int CategoryId { get; init; }
    public required string Currency { get; init; }
    public bool IsVisible { get; init; }
    public required IReadOnlyList<ProductVariantDto> ProductVariants { get; init; }
}

public sealed class ProductVariantDto
{
    public required string Sku { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
}