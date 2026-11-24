using Vendora.Services.Catalog.Application.Categories;

namespace Vendora.Services.Catalog.Api.Categories.Update;

public sealed class UpdateCategoryRequest
{
    public const string Pattern = "/update";

    public int Id { get; init; }
    public required string Name { get; init; }

    public UpdateCategory.Command ToCommand()
    {
        return new UpdateCategory.Command(Id, Name);
    }
}