namespace Vendora.Services.Catalog.Application.Products.ListByIds;

public class Response
{
    public required List<ProductDto> Products { get; init; }
}

public sealed class ProductDto
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public decimal Price { get; init; }
    public required string Currency { get; init; }
}
