using Vendora.Services.Catalog.Application.Features.CreateCategory;

namespace Vendora.Services.Catalog.Api.Features.CreateCategory;

public sealed class CreateCategoryRequest
{
    public required string Name { get; init; }

    public CreateCategoryCommand ToCommand()
    {
        return new CreateCategoryCommand
        {
            Name = Name
        };
    }
}