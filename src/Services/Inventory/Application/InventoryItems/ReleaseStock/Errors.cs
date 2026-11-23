namespace Vendora.Services.Inventory.Application.InventoryItems.ReleaseStock;

public static class Errors
{
    public static readonly Error ConcurrencyConflict = Error.Conflict(
        code: "releasing_stock.concurrency_conflict",
        message: "Stock was changed by another request. Please retry.");
}