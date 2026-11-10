namespace Vendora.Services.Catalog.Application.Products.ListByIds;

public class Query : IQuery<Response>
{
    public required List<long> ProductVariantReferences { get; init; }
}