using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Products.Reactivate;

public sealed class Command : ICommand
{
    public long Id { get; init; }
}