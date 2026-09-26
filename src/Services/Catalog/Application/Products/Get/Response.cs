namespace Vendora.Services.Catalog.Application.Products.Get;

public sealed class Response
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; } = null;
    public required string Brand { get; init; }
    public required CategoryDto Category { get; init; }
    public decimal Price { get; init; }
    public bool IsAvailable { get; init; }
    public int AvailableQuantity { get; init; }
    public required string Currency { get; init; }
    public required string Status { get; init; }
}

public sealed class CategoryDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
}