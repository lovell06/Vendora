namespace Vendora.Services.Inventory.Application.InventoryItems;

public static class CheckInventoryAvailability
{
    public sealed record Query(long ProductVariantId) : IQuery<Response>;

    public sealed record Response(long ProductVariantId, bool IsAvailable, int AvailableQuantity);

    public interface IQueryService
    {
        Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken);
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
                logger.LogWarning("Check Availability rejected because product is not existed.");

                return Result<Response>.Failure(new Error
                {
                    Code = "check_availability.not_found",
                    Message = "Product is not found.",
                    Type = ErrorType.NotFound
                });
            }

            return Result<Response>.Success(response);
        }
    }
}
