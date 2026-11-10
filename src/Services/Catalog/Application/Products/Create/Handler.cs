namespace Vendora.Services.Catalog.Application.Products.Create;

public sealed class Handler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IOutboxRepository outboxRepository,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        if (!await categoryRepository.ExistsByIdAsync(command.CategoryId, cancellationToken))
        {
            logger.LogWarning("Product creation rejected because category not existed.");

            return Result.Failure(new Error
            {
                Code = "product_creation.category_is_not_exists",
                Message = "Category is not exists.",
                Type = ErrorType.Validation
            });
        }

        var createdProductResult = Product.Create(
            name: command.Name,
            description: command.Description,
            brand: command.Brand,
            categoryId: command.CategoryId,
            currency: command.Currency,
            isVisible: command.IsVisible,
            createdAt: utcNow);
        
        if (createdProductResult.IsFailure)
        {
            logger.LogWarning(
                "Create product failed: {ErrorCode}", 
                createdProductResult.Error.Code);
            
            return Result.Failure(createdProductResult.Error);
        }

        var product = createdProductResult.Value;

        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        productRepository.Add(product);

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);

            outboxRepository.Add(new ProductCreatedEvent
            {
                Id = Guid.CreateVersion7(),
                OccurredAt = utcNow,
                ProductId = product.Id
            }, utcNow);

            foreach (var productVariant in command.ProductVariants)
            {
                var createVariantResult = ProductVariant.Create(
                    sku: productVariant.Sku,
                    name: productVariant.Name,
                    price: productVariant.Price,
                    productId: product.Id,
                    createdAt: utcNow);

                if (createVariantResult.IsFailure)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Failure(createVariantResult.Error);
                }

                var addVariantResult = product.AddVariant(createVariantResult.Value, utcNow);

                if (!addVariantResult.IsFailure) continue;
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(addVariantResult.Error);
            }
            
            await unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var productVariant in product.Variants)
            {
                outboxRepository.Add(new ProductVariantAddedEvent
                {
                    Id = Guid.CreateVersion7(),
                    OccurredAt = utcNow,
                    ProductId = product.Id,
                    Sku = productVariant.Sku,
                    VariantId = productVariant.Id
                }, utcNow);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }

        return Result.Success();
    }
}