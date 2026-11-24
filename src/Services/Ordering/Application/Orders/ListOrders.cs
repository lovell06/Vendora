namespace Vendora.Services.Ordering.Application.Orders;

public static class ListOrders
{
    public sealed record Query(Guid UserId, int Page, int Size) : IQuery<Response>;

    public sealed record Response(IReadOnlyList<OrderSummaryDto> Items, int Page, int Size, int TotalCount);

    public sealed record OrderSummaryDto(
        long Id,
        string Status,
        string Currency,
        decimal TotalAmount,
        DateTimeOffset CreatedAt);

    public interface IQueryService
    {
        Task<Response> ExecuteAsync(Query query, CancellationToken ct);
    }

    public static class Errors
    {
        public static readonly Error InvalidPageNumber = new()
        {
            Code = "listing_orders.invalid_page_number",
            Message = "Page number must be greater than zero.",
            Type = ErrorType.Validation
        };

        public static readonly Error InvalidPageSize = new()
        {
            Code = "listing_orders.invalid_page_number",
            Message = "Page size must be greater than zero.",
            Type = ErrorType.Validation
        };
    }

    public sealed class Handler(IQueryService queryService) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (query.Page < 1)
                return Result<Response>.Failure(Errors.InvalidPageNumber);

            if (query.Size < 1)
                return Result<Response>.Failure(Errors.InvalidPageSize);

            var response = await queryService.ExecuteAsync(query, cancellationToken);

            return Result<Response>.Success(response);
        }
    }
}
