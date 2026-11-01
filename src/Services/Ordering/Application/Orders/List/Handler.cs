namespace Vendora.Services.Ordering.Application.Orders.List;

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