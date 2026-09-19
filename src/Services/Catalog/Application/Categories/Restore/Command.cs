using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Categories.Restore;

public sealed class Command : ICommand
{
    public int Id { get; init; }
}