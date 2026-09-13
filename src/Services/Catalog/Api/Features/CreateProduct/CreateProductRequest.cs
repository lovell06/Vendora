using Vendora.Services.Catalog.Application.Features.CreateProduct;

namespace Vendora.Services.Catalog.Api.Features.CreateProduct;

public record CreateProductRequest(
    string Name,
    string? Description,
    string Brand,
    int CategoryId,
    decimal Price,
    bool IsVisible)
{
    public CreateProductCommand ToCommand()
    {
        return new CreateProductCommand(
            Name: Name,
            Description: Description,
            Brand: Brand,
            CategoryId: CategoryId,
            Price: Price,
            IsVisible: IsVisible);
    }
}