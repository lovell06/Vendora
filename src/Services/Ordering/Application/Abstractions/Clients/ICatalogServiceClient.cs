namespace Vendora.Services.Ordering.Application.Abstractions.Clients;

public interface ICatalogServiceClient
{
    Task<Result<ProductsInfoResponse>> GetProductVariantsInfoAsync(IReadOnlyList<long> productVariantIds, CancellationToken ct);
}

public sealed class ProductsInfoResponse
{
    public required IReadOnlyList<ProductVariantDto> ProductVariants { get; init; }
}

public sealed class ProductVariantDto
{
    public required long ProductId { get; init; }
    public required long VariantId { get; init; }
    public required string Sku { get; init; }
    public required string ProductName { get; init; }
    public required string VariantName { get; init; }
    public decimal Price { get; init; }
    public required string Currency { get; init; }
}