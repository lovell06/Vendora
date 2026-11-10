namespace Vendora.Services.Catalog.Application.Products.GetVariant;

public struct Query : IQuery<Response>
{
    public required long ProductVariantId { get; init; }
}