namespace Vendora.Services.Inventory.Application.InventoryItems.List;

public sealed class Response
{
    public required IReadOnlyList<InventoryItemDto> InventoryItems { get; init; }
    public int Page { get; init; }
    public int TotalCount { get; init; }
}

public sealed class InventoryItemDto
{
    public long ProductId { get; init; }
    public int OnHandQuantity { get; init; }
    public int ReservedQuantity { get; init; }
}