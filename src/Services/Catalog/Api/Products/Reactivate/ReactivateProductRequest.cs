using Vendora.Services.Catalog.Application.Products.Reactivate;

namespace Vendora.Services.Catalog.Api.Products.Reactivate;

public sealed class ReactivateProductRequest
{
    public const string Pattern = "/reactivate/{id}";

    public long Id { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Id = Id
        };
    }
}