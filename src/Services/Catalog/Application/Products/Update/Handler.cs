using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Application.Abstractions.Persistence;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Application.Products.Update;

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
        if (product is null)
        {
            logger.LogWarning("Product updates rejected because product not found.");

            return Result.Failure(new Error
            {
                Code = "product_update.failed",
                Message = "Product update failed.",
                Type = ErrorType.Failure
            });
        }

        product.Rename(cmd.Name, utcNow);
        product.ChangeDescription(cmd.Description!, utcNow);
        product.ChangeBrand(cmd.Brand, utcNow);
        product.ChangeCategory(cmd.CategoryId, utcNow);
        product.ChangePrice(cmd.Price, utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}