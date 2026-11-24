namespace Vendora.Services.Catalog.Application.Products;

public static class ListProductsByIds
{
    public record Query(List<long> ProductVariantReferences) : IQuery<Response>;

    public record Response(List<ProductVariantDto> ProductVariants);

    public sealed record ProductVariantDto(
        long ProductId,
        long VariantId,
        string ProductName,
        string VariantName,
        string Sku,
        decimal Price,
        string Currency);

    public interface IQueryService
    {
        Task<Response> ExecuteAsync(Query query, CancellationToken ct);
    }

    public static class Errors
    {
        public static readonly Error ProductIdsEmpty = new()
        {
            Code = "listing_product_by_ids.product_ids_empty",
            Message = "At least one product ID is required.",
            Type = ErrorType.Validation
        };

        public static readonly Error SomeProductsNotFound = new()
        {
            Code = "listing_product_by_ids.some_products_not_found",
            Message = "One or more requested products were not found.",
            Type = ErrorType.NotFound
        };
    }

    public sealed class Handler(IQueryService queryService) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (query.ProductVariantReferences.Count == 0)
                return Result<Response>.Failure(Errors.ProductIdsEmpty);

            var response = await queryService.ExecuteAsync(query, cancellationToken);

            if (query.ProductVariantReferences.Count != response.ProductVariants.Count)
                return Result<Response>.Failure(Errors.SomeProductsNotFound);

            return Result<Response>.Success(response);
        }
    }
}
