using Vendora.Services.Catalog.Application.Products.Restore;

namespace Vendora.Services.Catalog.Api.Products.Restore;

public class RestoreProductRequest
{
    public const string Pattern = "/restore/{id}";

    public long Id { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Id = Id
        };
    }
}