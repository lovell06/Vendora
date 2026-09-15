using Vendora.Services.Catalog.Application.Features.CreateProduct;

namespace Vendora.Services.Catalog.Api.Features.CreateProduct;

public sealed class CreateProductRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string Brand { get; init; }
    public int CategoryId { get; init; }
    public decimal Price { get; init; }
    public bool IsVisible { get; init; }

    public CreateProductCommand ToCommand()
    {
        return new CreateProductCommand
        {
            Name = Name,
            Description = Description,
            Brand = Brand,
            CategoryId = CategoryId,
            Price = Price,
            IsVisible = IsVisible
        };
    }
}