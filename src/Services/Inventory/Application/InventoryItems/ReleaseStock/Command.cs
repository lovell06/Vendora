namespace Vendora.Services.Inventory.Application.InventoryItems.ReleaseStock;

public sealed class Command : ICommand
{
    public required IReadOnlyList<ItemDto> Items { get; init; }
}

public sealed class ItemDto
{
    public required long ProductVariantId { get; init; }
    public required int Quantity { get; init; }
}