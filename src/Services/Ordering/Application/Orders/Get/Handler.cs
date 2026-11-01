namespace Vendora.Services.Ordering.Application.Orders.Get;

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