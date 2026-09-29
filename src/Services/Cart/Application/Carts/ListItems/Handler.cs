using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Cart.Application.Carts.ListItems;

public sealed class Handler(IListItemsQueryService queryService) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        var response = await queryService.ExecuteAsync(query, cancellationToken);

        if (response is null)
        {
            return Result<Response>.Failure(new Error
            {
                Code = "cart_list_items.cart_not_found",
                Message = "The user has not initialized a cart.",
                Type = ErrorType.NotFound
            });
        }

        return Result<Response>.Success(response);
    }
}