using Vendora.Services.Catalog.Application.Categories.Delete;

namespace Vendora.Services.Catalog.Api.Categories.Delete;

public sealed class DeleteCategoryRequest
{
    public const string Pattern = "/delete/{id}";

    public int Id { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Id = Id
        };
    }
}