using Vendora.Services.Catalog.Application.Products;

namespace Vendora.Services.Catalog.Api.Products.Update;

public sealed class UpdateProductRequest
{
    public const string Pattern = "/update";
    public long Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; } = null;
    public required string Brand { get; init; }
    public int CategoryId { get; init; }

    public UpdateProduct.Command ToCommand()
    {
        return new UpdateProduct.Command(Id, Name, Description, Brand, CategoryId);
    }
}