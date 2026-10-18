namespace Vendora.Services.Catalog.Application.Products.ListByIds;

public class Query : IQuery<Response>
{
    public required List<long> ProductIds { get; init; }
}
