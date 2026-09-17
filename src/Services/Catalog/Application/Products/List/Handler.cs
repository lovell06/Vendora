using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Catalog.Application.Products.List;

public class Handler(IListProductsQueryService queryService) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        var products = await queryService.GetByPageAsync(query.Page, query.Size, cancellationToken);

        return Result<Response>.Success(new Response { Products = products });
    }
}