using Vendora.Services.Catalog.Application.Products.Discontinue;

namespace Vendora.Services.Catalog.Api.Products.Discontinue;

public sealed class DiscontinueProductRequest
{
    public const string Pattern = "/discontinue/{id}";

    public long Id { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Id = Id
        };
    }
}