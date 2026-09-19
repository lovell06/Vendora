using Vendora.Services.Catalog.Application.Categories.Restore;

namespace Vendora.Services.Catalog.Api.Categories.Restore;

public sealed class RestoreCategoryRequest
{
    public const string Pattern = "/restore/{id}";

    public int Id { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Id = Id
        };
    }
}