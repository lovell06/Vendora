namespace Vendora.Services.Inventory.Application.InventoryItems.Reserve;

public sealed class Command : ICommand
{
    public required IReadOnlyList<ReservedItemDto> Items { get; init; }
}