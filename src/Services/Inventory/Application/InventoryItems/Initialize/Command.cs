namespace Vendora.Services.Inventory.Application.InventoryItems.Initialize;

public sealed class Command : ICommand
{
    public long ProductId { get; init; }
}