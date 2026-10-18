namespace Vendora.Services.Ordering.Application.Abstractions.Clients;

public interface ICatalogServiceClient
{
    Task<Result<ProductsInfoResponse>> GetProductsInfoAsync(IReadOnlyList<long> productIds, CancellationToken ct);
}

public sealed class ProductsInfoResponse
{
    public required IReadOnlyList<ProductDto> Products { get; init; }
}

public sealed class ProductDto
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public decimal Price { get; init; }
    public required string Currency { get; init; }
}