using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Application.Products.Restore;

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

        if (product is null)
        {
            logger.LogWarning("Product restore rejected because product never existed.");

            return Result.Failure(new Error
            {
                Code = "product_restore.not_found",
                Message = "Cannot restore the product never existed.",
                Type = ErrorType.NotFound
            });
        }

        product.Restore(utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}