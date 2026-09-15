using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Features.CreateCategory;

public sealed class Command : ICommand
{
    public required string Name { get; init; }
}