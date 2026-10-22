using Vendora.Services.Catalog.Application.Abstractions.Clients.Inventory;

namespace Vendora.Services.Catalog.Application.Products.Get;

public sealed class Handler(
    IQueryService getProductQueryService,
    IInventoryClient inventoryClient,
    ILogger<Handler> logger) : IQueryHandler<Query, Response>
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
    {
        var product = await getProductQueryService.ExecuteAsync(query, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Product retrieval rejected because not found.");

            return Result<Response>.Failure(new Error
            {
                Code = "product_retrieval.not_found",
                Message = "Product is not exists.",
                Type = ErrorType.NotFound
            });
        }

        var availabilityResult = await inventoryClient.CheckAvailabilityAsync(product.Id, cancellationToken);

        if (availabilityResult.IsFailure)
            return Result<Response>.Failure(availabilityResult.Error);

        return Result<Response>.Success(new Response
        {
            Id = product.Id,
            Name = product.Name,
            Brand = product.Brand,
            Description = product.Description,
            Price = product.Price,
            Currency = product.Currency,
            Category = product.Category,
            Status = product.Status,
            IsAvailable = availabilityResult.Value.IsAvailable,
            AvailableQuantity = availabilityResult.Value.AvailableQuantity
        });
    }
}