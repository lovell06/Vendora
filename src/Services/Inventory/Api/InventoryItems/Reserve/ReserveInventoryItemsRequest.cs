using Vendora.Services.Inventory.Application.InventoryItems.Reserve;

namespace Vendora.Services.Inventory.Api.InventoryItems.Reserve;

public sealed class ReserveInventoryItemsRequest
{
    public const string Pattern = "/reserve";
    public required List<ReservedItemDto> Items { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Items = [.. Items.DistinctBy(i => i.ProductId)]
        };
    }
}