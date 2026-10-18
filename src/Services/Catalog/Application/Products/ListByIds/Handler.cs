namespace Vendora.Services.Catalog.Application.Products.ListByIds;

public sealed class Handler(IQueryService queryService) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        if (query.ProductIds.Count == 0)
            return Result<Response>.Failure(Errors.ProductIdsEmpty);

        var response = await queryService.ExecuteAsync(query, cancellationToken);

        if (query.ProductIds.Count != response.Products.Count)
            return Result<Response>.Failure(Errors.SomeProductsNotFound);

        return Result<Response>.Success(response);
    }
}
