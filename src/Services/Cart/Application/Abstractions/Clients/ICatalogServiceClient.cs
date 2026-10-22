namespace Vendora.Services.Cart.Application.Abstractions.Clients;

public interface ICatalogServiceClient
{
    Task<Result> ProductExistsByIdAsync(long productId, CancellationToken ct);
}