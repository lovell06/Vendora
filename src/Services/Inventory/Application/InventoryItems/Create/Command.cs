using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Inventory.Application.InventoryItems.Create;

public sealed class Command : ICommand
{
    public int ProductId { get; init; }
    public int OnHandQuantity { get; init; }
}