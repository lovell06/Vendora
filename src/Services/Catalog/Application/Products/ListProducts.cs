namespace Vendora.Services.Catalog.Application.Products;

public static class ListProducts
{
    public sealed record Query(int? CategoryId, int Page, int Size) : IQuery<Response>;

    public sealed record Response(
        CategoryDto? Category,
        int Page,
        int TotalCount,
        IReadOnlyList<ProductDto> Products);

    public sealed record CategoryDto(int Id, string Name);

    public sealed record ProductDto(
        long Id,
        string Name,
        decimal MinPrice,
        decimal MaxPrice,
        string Currency,
        string ImageUrl);

    public interface IQueryService
    {
        Task<Response?> ExecuteAsync(
            Query query,
            CancellationToken cancellationToken);
    }

    public sealed class Handler(
        IQueryService queryService,
        ILogger<Handler> logger) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var response = await queryService.ExecuteAsync(query, cancellationToken);

            if (response is null)
            {
                logger.LogWarning("List products rejected because category does not exist.");

                return Result<Response>.Failure(new Error
                {
                    Code = "list_product.failed",
                    Message = "Category is not found.",
                    Type = ErrorType.NotFound
                });
            }

            return Result<Response>.Success(response);
        }
    }
}
