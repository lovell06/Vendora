using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Application.Products.Delete;

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
            logger.LogWarning("Product delete rejected because product not found.");

            return Result.Failure(new Error
            {
                Code = "product_delete.not_found",
                Message = "Cannot delete the product does not exist.",
                Type = ErrorType.NotFound
            });
        }

        product.Delete(utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}