using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Features.CreateProduct;

public sealed class CreateProductCommand : ICommand
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string Brand { get; init; }
    public int CategoryId { get; init; }
    public decimal Price { get; init; }
    public bool IsVisible { get; init; }
}