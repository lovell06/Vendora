using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Categories.Create
{
    public sealed class Command : ICommand
    {
        public required string Name { get; init; }
    }
}