using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Application.Products.Discontinue;

public sealed class Handler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow().UtcDateTime;

        var product = await productRepository.GetByIdAsync(cmd.Id, cancellationToken);

        if (product is null || product.IsDeleted)
        {
            logger.LogWarning(
                "Product discontinue rejected because the product {reason}",
                product is null ? "does not exist." : "is deleted.");

            return Result.Failure(new Error
            {
                Code = "product_discontinue.failed",
                Message = "Cannot discontinue the product never existed or deleted.",
                Type = ErrorType.NotFound
            });
        }

        product.Discontinue(utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}