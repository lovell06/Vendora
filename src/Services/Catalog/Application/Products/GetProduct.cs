using Vendora.Services.Catalog.Application.Abstractions.Clients.Inventory;

namespace Vendora.Services.Catalog.Application.Products;

public static class GetProduct
{
    public sealed record Query(long Id) : IQuery<Response>;

    public sealed record Response(ProductDto Product);

    public sealed record ProductDto(
        long Id,
        string Name,
        string? Description,
        string Brand,
        CategoryDto Category,
        string Currency,
        string Status,
        IReadOnlyList<ProductVariantDto> Variants,
        IReadOnlyList<ProductImageDto> Images);

    public sealed record ProductImageDto(long Id, string Url, int SortOrder);

    public sealed record ProductVariantDto(
        string Sku,
        string Name,
        decimal Price,
        IReadOnlyList<long> ImageIds);

    public sealed record CategoryDto(int Id, string Name);

    public interface IQueryService
    {
        Task<ProductDto?> ExecuteAsync(Query query, CancellationToken cancellationToken);
    }

    public sealed class Handler(
        IQueryService getProductQueryService,
        IInventoryClient inventoryClient,
        ILogger<Handler> logger) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var product = await getProductQueryService.ExecuteAsync(query, cancellationToken);

            if (product is null)
            {
                logger.LogWarning("Product retrieval rejected because not found.");

                return Result<Response>.Failure(new Error
                {
                    Code = "product_retrieval.not_found",
                    Message = "Product is not exists.",
                    Type = ErrorType.NotFound
                });
            }

            var availabilityResult = await inventoryClient.CheckAvailabilityAsync(product.Id, cancellationToken);

            if (availabilityResult.IsFailure)
                return Result<Response>.Failure(availabilityResult.Error);

            return Result<Response>.Success(new Response(product));
        }
    }
}
