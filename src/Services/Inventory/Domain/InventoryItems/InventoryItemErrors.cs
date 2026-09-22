using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Inventory.Domain.InventoryItems;

public static class InventoryItemErrors
{
    public static Error QuantityChangeCannotBeZero => new()
    {
        Code = "inventory_item.quantity_change_cannot_be_zero",
        Message = "Quantity Change must be differ zero.",
        Type = ErrorType.Validation
    };
    
    public static Error InvalidProductId => new()
    {
        Code = "inventory_item.invalid_product_id",
        Message = "Product ID must be positive.",
        Type = ErrorType.Validation
    };

    public static Error InvalidInitialQuantity => new()
    {
        Code = "inventory_item.invalid_initial_quantity",
        Message = "Initial quantity must be greater than zero.",
        Type = ErrorType.Validation
    };

    public static Error InvalidQuantity => new()
    {
        Code = "inventory_item.invalid_quantity",
        Message = "Quantity must be greater than zero.",
        Type = ErrorType.Validation
    };

    public static Error InsufficientAvailableStock => new()
    {
        Code = "inventory_item.insufficient_available_stock",
        Message = "Insufficient available stock.",
        Type = ErrorType.Validation
    };

    public static Error InsufficientReservedStock => new()
    {
        Code = "inventory_item.insufficient_reserved_stock",
        Message = "Insufficient reserved stock.",
        Type = ErrorType.Validation
    };
}