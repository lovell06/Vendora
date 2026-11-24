namespace Vendora.Services.Catalog.Application.Products;

public static class GetProductVariant
{
    public readonly record struct Query(long ProductVariantId) : IQuery<Response>;

    public sealed record Response(ProductVariantDto ProductVariant);

    public sealed record ProductVariantDto(long ProductId, long ProductVariantId, string Sku);

    public interface IQueryService
    {
        Task<Response?> ExecuteAsync(Query query, CancellationToken ct);
    }

    public sealed class Handler(IQueryService queryService) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var response = await queryService.ExecuteAsync(query, cancellationToken);

            if (response is null)
                return Result<Response>.Failure(new Error
                {
                    Code = "getting_product_variant.not_found",
                    Message = "Product variant not found",
                    Type = ErrorType.NotFound
                });

            return Result<Response>.Success(response);
        }
    }
}
