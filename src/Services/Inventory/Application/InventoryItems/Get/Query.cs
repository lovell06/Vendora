namespace Vendora.Services.Inventory.Application.InventoryItems.Get;

public sealed class Query : IQuery<Response>
{
    public long ProductVariantId { get; init; }
}