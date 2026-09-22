using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Application.Products.Reactivate;

public sealed class Handler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        var product = await productRepository.GetByIdAsync(cmd.Id, cancellationToken);

        if (product is null || product.IsDeleted)
        {
            logger.LogWarning(
                "Product activate rejected because the product {reason}",
                product is null ? "does not exist." : "is deleted.");
            
            return Result.Failure(new Error
            {
                Code = "product_activate.failed",
                Message = "Cannot reactivate the product does not exist or deleted",
                Type = ErrorType.NotFound
            });
        }

        product.Reactivate(utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}