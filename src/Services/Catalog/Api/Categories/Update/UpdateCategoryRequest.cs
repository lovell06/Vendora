using Vendora.Services.Catalog.Application.Categories.Update;

namespace Vendora.Services.Catalog.Api.Categories.Update;

public sealed class UpdateCategoryRequest
{
    public const string Pattern = "/update";

    public int Id { get; init; }
    public required string Name { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Id = Id,
            Name = Name
        };
    }
}