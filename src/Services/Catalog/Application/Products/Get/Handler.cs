using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Catalog.Application.Products.Get;

public sealed class Handler(
    IGetProductQueryService getProductQueryService,
    ILogger<Handler> logger) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        var response = await getProductQueryService.GetProductById(query.Id, cancellationToken);

        if (response is null)
        {
            logger.LogWarning("Product retrieval rejected because not found.");

            return Result<Response>.Failure(new Error
            {
                Code = "product_retrieval.not_found",
                Message = "Product is not exists.",
                Type = ErrorType.NotFound
            });
        }

        return Result<Response>.Success(response);
    }
}