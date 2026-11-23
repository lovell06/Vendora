namespace Vendora.Services.Inventory.Application.InventoryItems.Reserve;

public static class Errors
{
    public static readonly Error ItemNotFound = new()
    {
        Code = "inventory_item_reserving.item_not_found",
        Message = "Inventory item not found.",
        Type = ErrorType.NotFound
    };

    public static readonly Error ItemsEmpty = new()
    {
        Code = "inventory_item_reserving.item_empty",
        Message = "Inventory items must be not empty.",
        Type = ErrorType.Validation
    };

    public static readonly Error ConcurrencyConflict = Error.Conflict(
        code: "inventory_item_reserving.concurrency_conflict",
        message: "Stock was changed by another request. Please try again.");
}