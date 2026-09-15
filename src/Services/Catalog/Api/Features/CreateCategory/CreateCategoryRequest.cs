using Vendora.Services.Catalog.Application.Features.CreateCategory;

namespace Vendora.Services.Catalog.Api.Features.CreateCategory;

public sealed class CreateCategoryRequest
{
    public required string Name { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Name = Name
        };
    }
}