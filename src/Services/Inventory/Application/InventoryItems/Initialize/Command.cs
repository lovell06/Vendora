using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Inventory.Application.InventoryItems.Initialize;

public sealed class Command : ICommand
{
    public int ProductId { get; init; }
}