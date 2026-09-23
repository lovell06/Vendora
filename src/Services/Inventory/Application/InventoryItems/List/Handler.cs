using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Inventory.Application.InventoryItems.List;

public sealed class Handler(IListInventoryItemsQueryService queryService) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        return Result<Response>.Success(
            await queryService.ExecuteAsync(query, cancellationToken));
    }
}