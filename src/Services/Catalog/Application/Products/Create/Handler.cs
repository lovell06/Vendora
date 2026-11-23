using Vendora.Services.Catalog.Application.Abstractions.Storage;

namespace Vendora.Services.Catalog.Application.Products.Create;

public sealed class Handler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IOutboxRepository outboxRepository,
    IObjectStorage objectStorage,
    IUnitOfWork unitOfWork,
    ILogger<Handler> logger,
    TimeProvider clock) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        if (command.ProductImageUploads.Count == 0)
            return Result.Failure(Error.Validation(
                code: "product_creation.images_empty",
                message: "The product require has least one image."));

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

        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        var product = createdProductResult.Value;

        productRepository.Add(product);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var commitStarted = false;
        var addedImageKeys = new List<string>();
        try
        {
            for (var index = 0; index < command.ProductImageUploads.Count; ++index)
            {
                var file = command.ProductImageUploads[index];

                await using var fileContent = file.OpenReadStream();

                var objectKey = await objectStorage.StoreAsync(
                    content: fileContent,
                    contentType: file.ContentType,
                    prefix: $"products/{product.Id}",
                    ct: cancellationToken);

                addedImageKeys.Add(objectKey);

                var createProductImageResult = ProductImage.Create(
                    productId: product.Id,
                    objectKey: objectKey,
                    contentType: file.ContentType,
                    sizeBytes: file.SizeBytes,
                    isPrimary: file.IsPrimary,
                    sortedOrder: index + 1,
                    creationDate: utcNow
                );

                if (createProductImageResult.IsFailure)
                    return Result.Failure(createProductImageResult.Error);

                var addImageResult = product.AddImage(createProductImageResult.Value, utcNow);

                if (addImageResult.IsFailure)
                    return Result.Failure(addImageResult.Error);
            }

            try
            {
                await unitOfWork.SaveChangesAsync(cancellationToken);

                // outboxRepository.Add(new ProductCreatedEvent
                // {
                //     Id = Guid.CreateVersion7(),
                //     OccurredAt = utcNow,
                //     ProductId = product.Id
                // }, utcNow);

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

                    var variant = createVariantResult.Value;

                    for (var index = 0; index < productVariant.ImageIndexes.Count; index++)
                    {
                        var imageId = productVariant.ImageIndexes[index];
                        var assignedImage = product.Images[imageId];

                        var assignImageResult = product.AssignImageToVariant(
                            image: assignedImage,
                            variant: variant,
                            isPrimary: index == 0,
                            updatedAt: utcNow);

                        if (assignImageResult.IsFailure)
                            return Result.Failure(assignImageResult.Error);
                    }

                    var addVariantResult = product.AddVariant(variant, utcNow);

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

                commitStarted = true;

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(CancellationToken.None);
                throw;
            }
        }
        finally
        {
            if (!commitStarted && addedImageKeys.Count > 0)
            {
                using var cleanupTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                foreach (var key in addedImageKeys)
                {
                    try
                    {
                        await objectStorage.DeleteIfExistsAsync(key, cleanupTimeout.Token);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Failed to clean up product image {ObjectKey}", key);
                    }
                }
            }

        }

        return Result.Success();
    }
}