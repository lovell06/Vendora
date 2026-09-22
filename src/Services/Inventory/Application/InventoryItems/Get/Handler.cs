using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Inventory.Application.InventoryItems.Get;

public sealed class Handler(IGetInventoryItemQueryService queryService) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        var response = await queryService.ExecuteAsync(query, cancellationToken);

        if (response is null)
            return Result<Response>.Failure(new Error
            {
                Code = "inventory_item_get.not_found",
                Message = "Inventory Item is not found.",
                Type = ErrorType.NotFound
            });
        
        return Result<Response>.Success(response);
    }
}