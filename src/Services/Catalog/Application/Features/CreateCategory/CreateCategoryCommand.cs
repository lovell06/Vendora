using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Features.CreateCategory;

public record CreateCategoryCommand(string Name) : ICommand;