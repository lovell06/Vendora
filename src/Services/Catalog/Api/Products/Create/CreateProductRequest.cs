using Vendora.Services.Catalog.Application.Products;

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
    public required IReadOnlyList<CreateProduct.ProductVariantDto> ProductVariants { get; init; }
    public required IReadOnlyList<IFormFile> Images { get; init; }

    public CreateProduct.Command ToCommand()
    {
        return new CreateProduct.Command(
            Name,
            Description,
            Brand,
            CategoryId,
            Currency,
            IsVisible,
            ProductVariants,
            [
                .. Images.Select((file, index) => new CreateProduct.ProductImageUploadDto(
                    file.OpenReadStream,
                    file.ContentType,
                    file.Length,
                    index == 0))
            ]);
    }
}