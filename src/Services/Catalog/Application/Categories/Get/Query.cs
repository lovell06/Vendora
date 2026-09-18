using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Catalog.Application.Categories.Get;

public sealed class Query : IQuery<Response>
{
    public int Id { get; init; }
}