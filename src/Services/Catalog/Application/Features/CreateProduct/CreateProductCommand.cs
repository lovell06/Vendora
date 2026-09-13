using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Features.CreateProduct;

public record CreateProductCommand(
    string Name,
    string? Description,
    string Brand,
    int CategoryId,
    decimal Price,
    bool IsVisible) : ICommand;