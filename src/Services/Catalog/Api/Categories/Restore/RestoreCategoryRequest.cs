using Vendora.Services.Catalog.Application.Categories;

namespace Vendora.Services.Catalog.Api.Categories.Restore;

public sealed class RestoreCategoryRequest
{
    public const string Pattern = "/restore/{id}";

    public int Id { get; init; }

    public RestoreCategory.Command ToCommand()
    {
        return new RestoreCategory.Command(Id);
    }
}