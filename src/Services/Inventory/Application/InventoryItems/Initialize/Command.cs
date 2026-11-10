namespace Vendora.Services.Inventory.Application.InventoryItems.Initialize;

public sealed class Command : ICommand
{
    public required long ProductId { get; init; }
    public required long ProductVariantId { get; init; }
    public required string Sku { get; init; }
}