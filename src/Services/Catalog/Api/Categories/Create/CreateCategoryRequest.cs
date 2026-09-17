using Vendora.Services.Catalog.Application.Categories.Create;

namespace Vendora.Services.Catalog.Api.Categories.Create;

public sealed class CreateCategoryRequest
{
    public const string Pattern = "/create";
    public required string Name { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Name = Name
        };
    }
}