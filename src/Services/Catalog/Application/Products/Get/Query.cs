using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Products.Get;

public sealed class Query : IQuery<Response>
{
    public long Id { get; init; }
}