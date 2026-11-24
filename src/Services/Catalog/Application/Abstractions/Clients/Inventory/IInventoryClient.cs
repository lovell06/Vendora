namespace Vendora.Services.Catalog.Application.Abstractions.Clients.Inventory;

public interface IInventoryClient
{
    Task<Result<CheckAvailabilityResult>> CheckAvailabilityAsync(long productVariantId, CancellationToken cancellationToken);
}

public sealed class CheckAvailabilityResult
{
    public long ProductVariantId { get; init; }
    public bool IsAvailable { get; init; }
    public int AvailableQuantity { get; init; }
}