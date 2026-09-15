using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Features.CreateCategory;

public sealed class CreateCategoryCommand : ICommand
{
    public required string Name { get; init; }
}