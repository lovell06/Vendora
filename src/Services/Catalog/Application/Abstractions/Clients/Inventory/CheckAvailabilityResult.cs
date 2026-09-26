namespace Vendora.Services.Catalog.Application.Abstractions.Clients.Inventory;

public sealed class CheckAvailabilityResult
{
    public long ProductId { get; init; }
    public bool IsAvailable { get; init; }
    public int AvailableQuantity { get; init; }
}