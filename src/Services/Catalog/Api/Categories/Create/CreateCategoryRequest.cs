using Vendora.Services.Catalog.Application.Categories;

namespace Vendora.Services.Catalog.Api.Categories.Create;

public sealed class CreateCategoryRequest
{
    public const string Pattern = "/create";
    public required string Name { get; init; }

    public CreateCategory.Command ToCommand()
    {
        return new CreateCategory.Command(Name);
    }
}