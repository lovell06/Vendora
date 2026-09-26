using Vendora.Services.Catalog.Application.Abstractions.Authentication;

namespace Vendora.Services.Catalog.Application.Abstractions.Clients.Inventory;

public interface IInventoryClient
{
    Task InitializeStockAsync(long productId, ICurrentUser currentUser, CancellationToken cancellationToken);
    Task<CheckAvailabilityResult> CheckAvailabilityAsync(long productId, CancellationToken cancellationToken);
}