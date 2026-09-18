using Vendora.Services.Catalog.Application.Products.Update;

namespace Vendora.Services.Catalog.Api.Products.Update;

public sealed class UpdateProductRequest
{
    public const string Pattern = "/update";
    public long Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; } = null;
    public required string Brand { get; init; }
    public int CategoryId { get; init; }
    public decimal Price { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Brand = Brand,
            CategoryId = CategoryId,
            Price = Price
        };
    }
}