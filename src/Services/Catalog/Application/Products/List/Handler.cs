using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Catalog.Application.Products.List;

public sealed class Handler(
    IListProductsQueryService queryService,
    ILogger<Handler> logger) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        var response = await queryService.ExecuteAsync(query, cancellationToken);

        if (response is null)
        {
            logger.LogWarning("List products rejected because category does not exist.");

            return Result<Response>.Failure(new Error
            {
                Code = "list_product.failed",
                Message = "Category is not found.",
                Type = ErrorType.NotFound
            });
        }

        return Result<Response>.Success(response);
    }
}