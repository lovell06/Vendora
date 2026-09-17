using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Products.List;

public sealed class Query : IQuery<Response>
{
    public int Page { get; init; }
    public int Size { get; init; }
}