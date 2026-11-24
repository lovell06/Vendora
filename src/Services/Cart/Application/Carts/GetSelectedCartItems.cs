namespace Vendora.Services.Cart.Application.Carts;

public static class GetSelectedCartItems
{
    public sealed record Query(Guid UserId, List<long> ProductVariantIds) : IQuery<Response>;
    public sealed record Response(List<CartItemDto> Items);
    public sealed record CartItemDto(long ProductVariantId, int Quantity);

    public sealed class Handler(IQueryService queryService) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (query.ProductVariantIds.Count == 0)
                return Result<Response>.Failure(Errors.SelectedItemsIsEmpty);
            
            var response = await queryService.ExecuteAsync(query, cancellationToken);

            if (response is null)
                return Result<Response>.Failure(Errors.UserNotFound);
            
            if (query.ProductVariantIds.Count != response.Items.Count)
                return Result<Response>.Failure(Errors.ProductsNotFoundInCart);

            return Result<Response>.Success(response);
        }
    }

    public interface IQueryService
    {
        Task<Response?> ExecuteAsync(Query query, CancellationToken ct);
    }

    public static class Errors
    {
        public static readonly Error UserNotFound = new()
        {
            Code = "getting_selected_item.user_not_found",
            Message = "The user has not initialized a cart.",
            Type = ErrorType.NotFound
        };

        public static readonly Error SelectedItemsIsEmpty = new()
        {
            Code = "getting_selected_item.selected_items_is_empty",
            Message = "The selected cart items must be not empty.",
            Type = ErrorType.Validation
        };

        public static readonly Error ProductsNotFoundInCart = new()
        {
            Code = "getting_selected_item.product_not_found_in_cart",
            Message = "Some products not found in cart.",
            Type = ErrorType.NotFound
        };
    }
}