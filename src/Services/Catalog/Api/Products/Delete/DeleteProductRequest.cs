using Vendora.Services.Catalog.Application.Products.Delete;

namespace Vendora.Services.Catalog.Api.Products.Delete;

public sealed class DeleteProductRequest
{
    public const string Pattern = "/delete/{id}";

    public long Id { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Id = Id
        };
    }
}