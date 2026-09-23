using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Inventory.Application.InventoryItems.List;

public sealed class Query : IQuery<Response>
{
    public int Page { get; init; }
    public int Size { get; init; }
}