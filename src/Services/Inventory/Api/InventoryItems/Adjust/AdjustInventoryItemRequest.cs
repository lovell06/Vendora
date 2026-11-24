using Vendora.Services.Inventory.Application.InventoryItems;

namespace Vendora.Services.Inventory.Api.InventoryItems.Adjust;

public sealed class AdjustInventoryItemRequest
{
    public const string Pattern = "/adjust";

    public long InventoryItemId { get; init; }
    public int QuantityChange { get; init; }
    public required string Reason { get; init; }

    public AdjustInventoryItem.Command ToCommand(Guid createdBy)
    {
        return new AdjustInventoryItem.Command(
            InventoryItemId,
            QuantityChange,
            Reason,
            createdBy);
    }
}