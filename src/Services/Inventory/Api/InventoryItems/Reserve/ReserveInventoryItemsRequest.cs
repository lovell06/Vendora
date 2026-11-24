using Vendora.Services.Inventory.Application.InventoryItems;

namespace Vendora.Services.Inventory.Api.InventoryItems.Reserve;

public sealed class ReserveInventoryItemsRequest
{
    public const string Pattern = "/reserve";
    public required List<ReserveInventoryItems.ReservedItemDto> Items { get; init; }

    public ReserveInventoryItems.Command ToCommand()
    {
        return new ReserveInventoryItems.Command(
            [.. Items.DistinctBy(i => i.ProductVariantId)]);
    }
}