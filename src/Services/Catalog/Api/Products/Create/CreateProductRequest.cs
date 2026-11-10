using Vendora.Services.Catalog.Application.Products.Create;

namespace Vendora.Services.Catalog.Api.Products.Create;

public sealed class CreateProductRequest
{
    public const string Pattern = "/create";
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string Brand { get; init; }
    public int CategoryId { get; init; }
    public required string Currency { get; init; }
    public bool IsVisible { get; init; }
    public required IReadOnlyList<ProductVariantDto> ProductVariants { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Name = Name,
            Description = Description,
            Brand = Brand,
            CategoryId = CategoryId,
            Currency = Currency,
            IsVisible = IsVisible,
            ProductVariants = ProductVariants
        };
    }
}