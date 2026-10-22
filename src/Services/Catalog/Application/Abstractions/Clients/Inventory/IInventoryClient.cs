namespace Vendora.Services.Catalog.Application.Abstractions.Clients.Inventory;

public interface IInventoryClient
{
    Task<Result<CheckAvailabilityResult>> CheckAvailabilityAsync(long productId, CancellationToken cancellationToken);
}

public sealed class CheckAvailabilityResult
{
    public long ProductId { get; init; }
    public bool IsAvailable { get; init; }
    public int AvailableQuantity { get; init; }
}