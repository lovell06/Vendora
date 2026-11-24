using Vendora.Services.Catalog.Application.Categories;

namespace Vendora.Services.Catalog.Api.Categories.Delete;

public sealed class DeleteCategoryRequest
{
    public const string Pattern = "/delete/{id}";

    public int Id { get; init; }

    public DeleteCategory.Command ToCommand()
    {
        return new DeleteCategory.Command(Id);
    }
}