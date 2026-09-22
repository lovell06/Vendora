namespace Vendora.Services.Inventory.Application.InventoryItems.Get;

public sealed class Response
{
    public long ProductId { get; init; }
    public int OnHandQuantity { get; init; }
    public int ReservedQuantity { get; init; }
}