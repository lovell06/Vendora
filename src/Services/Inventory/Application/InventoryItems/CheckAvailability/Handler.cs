using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Inventory.Application.InventoryItems.CheckAvailability;

public sealed class Handler(
    ICheckAvailabilityQueryService queryService,
    ILogger<Handler> logger) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        var response = await queryService.ExecuteAsync(query, cancellationToken);

        if (response is null)
        {
            logger.LogWarning("Check Availability rejected because product is not existed.");

            return Result<Response>.Failure(new Error
            {
                Code = "check_availability.not_found",
                Message = "Product is not found.",
                Type = ErrorType.NotFound
            });
        }

        return Result<Response>.Success(response);
    }
}