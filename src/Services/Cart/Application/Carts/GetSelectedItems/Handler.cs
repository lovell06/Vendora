namespace Vendora.Services.Cart.Application.Carts.GetSelectedItems;

public sealed class Handler(IGetSelectedItemsQueryService queryService) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        if (query.ProductIds.Count == 0)
            return Result<Response>.Failure(Errors.SelectedItemsIsEmpty);
        
        var response = await queryService.ExecuteAsync(query, cancellationToken);

        if (response is null)
            return Result<Response>.Failure(Errors.UserNotFound);
        
        if (query.ProductIds.Count != response.Items.Count)
            return Result<Response>.Failure(Errors.ProductsNotFoundInCart);

        return Result<Response>.Success(response);
    }
}