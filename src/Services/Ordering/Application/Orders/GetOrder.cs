namespace Vendora.Services.Ordering.Application.Orders;

public static class GetOrder
{
    public sealed record Query(Guid UserId, long OrderId) : IQuery<Response>;

    public sealed record Response(OrderDetailDto Order);

    public sealed record OrderDetailDto(
        long Id,
        string Status,
        string Currency,
        decimal TotalAmount,
        DateTimeOffset CreatedAt,
        IReadOnlyList<OrderItemDto> Items);

    public sealed record OrderItemDto(
        long ProductId,
        string ProductName,
        decimal UnitPrice,
        int Quantity,
        decimal LineTotal);

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
            {
                return Result<Response>.Failure(new Error
                {
                    Code = "getting_order.not_found",
                    Message = "Order is not found.",
                    Type = ErrorType.NotFound
                });
            }

            return Result<Response>.Success(response);
        }
    }
}
