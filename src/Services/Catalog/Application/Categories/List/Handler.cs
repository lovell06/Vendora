using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Catalog.Application.Categories.List;

public sealed class Handler(IListCategoryQueryService listCategoryQueryService) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        var response = await listCategoryQueryService.ExecuteAsync(query, cancellationToken);
        return Result<Response>.Success(response);
    }
}