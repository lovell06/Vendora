using Vendora.Services.Inventory.Application.InventoryItems.Adjust;

namespace Vendora.Services.Inventory.Api.InventoryItems.Adjust;

public sealed class AdjustInventoryItemRequest
{
    public const string Pattern = "/adjust";

    public long ProductId { get; init; }
    public int QuantityChange { get; init; }
    public required string Reason { get; init; }

    public Command ToCommand(Guid createdBy)
    {
        return new Command
        {
            ProductId = ProductId,
            QuantityChange = QuantityChange,
            Reason = Reason,
            CreatedBy = createdBy
        };
    }
}