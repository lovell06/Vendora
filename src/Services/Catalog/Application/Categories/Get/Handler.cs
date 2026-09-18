using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Catalog.Application.Categories.Get;

public sealed class Handler(
    IGetCategoryQueryService getCategoryQueryService,
    ILogger<Handler> logger) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        var response = await getCategoryQueryService.GetByIdAsync(query.Id, cancellationToken);

        if (response is null)
        {
            logger.LogWarning("Category retrieval rejected because category not found.");

            return Result<Response>.Failure(new Error
            {
                Code = "category_retrieval.not_found",
                Message = "Category is not found.",
                Type = ErrorType.NotFound
            });
        }

        return Result<Response>.Success(response);
    }
}