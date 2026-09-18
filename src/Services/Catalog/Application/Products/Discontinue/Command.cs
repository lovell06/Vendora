using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Products.Discontinue;

public sealed class Command : ICommand
{
    public long Id { get; init; }
}