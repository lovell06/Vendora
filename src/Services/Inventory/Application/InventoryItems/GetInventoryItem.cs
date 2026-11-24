namespace Vendora.Services.Inventory.Application.InventoryItems;

public static class GetInventoryItem
{
    public sealed record Query(long ProductVariantId) : IQuery<Response>;

    public sealed record Response(
        long ProductVariantId,
        int OnHandQuantity,
        int ReservedQuantity,
        DateTimeOffset CreatedAt,
        DateTimeOffset? UpdatedAt);

    public interface IQueryService
    {
        Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken);
    }

    public sealed class Handler(IQueryService queryService) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var response = await queryService.ExecuteAsync(query, cancellationToken);

            if (response is null)
                return Result<Response>.Failure(new Error
                {
                    Code = "inventory_item_get.not_found",
                    Message = "Inventory Item is not found.",
                    Type = ErrorType.NotFound
                });

            return Result<Response>.Success(response);
        }
    }
}
