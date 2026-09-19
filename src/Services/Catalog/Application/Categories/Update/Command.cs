using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Categories.Update;

public sealed class Command : ICommand
{
    public int Id { get; init; }
    public required string Name { get; init; }
}