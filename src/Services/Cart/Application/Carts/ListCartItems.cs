namespace Vendora.Services.Cart.Application.Carts;

public static class ListCartItems
{
    public sealed record Query(Guid UserId) : IQuery<Response>;
    public sealed record Response(IReadOnlyList<ItemDto> Items);
    public sealed record ItemDto(long ProductVariantId, int Quantity);

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
                    Code = "listing_cart_items.cart_not_found",
                    Message = "The user has not initialized a cart.",
                    Type = ErrorType.NotFound
                });
            }

            return Result<Response>.Success(response);
        }
    }
}