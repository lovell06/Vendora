using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Products.Update;

public sealed class Command : ICommand
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; } = null;
    public required string Brand { get; init; }
    public int CategoryId { get; init; }
    public decimal Price { get; init; }
}