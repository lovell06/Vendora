using Vendora.Services.Catalog.Application.Features.CreateCategory;

namespace Vendora.Services.Catalog.Api.Features.CreateCategory;

public record CreateCategoryRequest(string Name)
{
    public CreateCategoryCommand ToCommand()
    {
        return new CreateCategoryCommand(Name);
    }
}