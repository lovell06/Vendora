using Vendora.Services.Catalog.Application.Abstractions.Storage;

namespace Vendora.Services.Catalog.Application.Products;

public static class CreateProduct
{
    public sealed record Command(
        string Name,
        string? Description,
        string Brand,
        int CategoryId,
        string Currency,
        bool IsVisible,
        IReadOnlyList<ProductVariantDto> ProductVariants,
        IReadOnlyList<ProductImageUploadDto> ProductImageUploads) : ICommand<Response>;

    public sealed record Response(long ProductId);

    public const int MaxImageCount = 10;
    public const long MaxImageSizeBytes = 5 * 1024 * 1024;
    public static readonly IReadOnlySet<string> AllowedImageContentTypes =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "image/jpeg", "image/png", "image/webp" };

    public sealed record ProductImageUploadDto(
        Func<Stream> OpenReadStream,
        string ContentType,
        long SizeBytes,
        bool IsPrimary);

    public sealed class ProductVariantDto
    {
        public required string Sku { get; init; }
        public required string Name { get; init; }
        public required decimal Price { get; init; }
        public required IReadOnlyList<int> ImageIndexes { get; init; }
    }

    public sealed class Handler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IOutboxRepository outboxRepository,
        IObjectStorage objectStorage,
        IUnitOfWork unitOfWork,
        ILogger<Handler> logger,
        TimeProvider clock) : ICommandHandler<Command, Response>
    {
        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var utcNow = clock.GetUtcNow();

            // All input checks run before anything is written to the database or
            // uploaded to storage, so a bad request never leaves orphan blobs behind.
            var validationError = Validate(command);
            if (validationError is not null)
            {
                logger.LogWarning("Product creation rejected: {ErrorCode}", validationError.Code);
                return Result<Response>.Failure(validationError);
            }

            if (!await categoryRepository.ExistsByIdAsync(command.CategoryId, cancellationToken))
            {
                logger.LogWarning("Product creation rejected because category not existed.");

                return Result<Response>.Failure(new Error
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

                return Result<Response>.Failure(createdProductResult.Error);
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
                        return Result<Response>.Failure(createProductImageResult.Error);

                    var addImageResult = product.AddImage(createProductImageResult.Value, utcNow);

                    if (addImageResult.IsFailure)
                        return Result<Response>.Failure(addImageResult.Error);
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
                            return Result<Response>.Failure(createVariantResult.Error);
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
                                return Result<Response>.Failure(assignImageResult.Error);
                        }

                        var addVariantResult = product.AddVariant(variant, utcNow);

                        if (!addVariantResult.IsFailure) continue;
                        await transaction.RollbackAsync(cancellationToken);
                        return Result<Response>.Failure(addVariantResult.Error);
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

            return Result<Response>.Success(new Response(product.Id));
        }

        private static Error? Validate(Command command)
        {
            var images = command.ProductImageUploads;

            if (images.Count == 0)
                return Error.Validation(
                    code: "product_creation.images_empty",
                    message: "The product requires at least one image.");

            if (images.Count > MaxImageCount)
                return Error.Validation(
                    code: "product_creation.too_many_images",
                    message: $"A product can have at most {MaxImageCount} images.");

            for (var index = 0; index < images.Count; index++)
            {
                var image = images[index];

                if (!AllowedImageContentTypes.Contains(image.ContentType.Trim()))
                    return Error.Validation(
                        code: "product_creation.image_type_not_allowed",
                        message: $"Image {index} has type '{image.ContentType}'. Allowed types: {string.Join(", ", AllowedImageContentTypes)}.");

                if (image.SizeBytes <= 0)
                    return Error.Validation(
                        code: "product_creation.image_empty",
                        message: $"Image {index} is empty.");

                if (image.SizeBytes > MaxImageSizeBytes)
                    return Error.Validation(
                        code: "product_creation.image_too_large",
                        message: $"Image {index} is larger than {MaxImageSizeBytes / (1024 * 1024)} MB.");
            }

            foreach (var variant in command.ProductVariants)
            {
                var indexes = variant.ImageIndexes;

                foreach (var imageIndex in indexes)
                {
                    if (imageIndex < 0 || imageIndex >= images.Count)
                        return Error.Validation(
                            code: "product_creation.image_index_out_of_range",
                            message: $"Variant '{variant.Sku}' references image index {imageIndex}, but only indexes 0 to {images.Count - 1} exist.");
                }

                if (indexes.Distinct().Count() != indexes.Count)
                    return Error.Validation(
                        code: "product_creation.image_index_duplicated",
                        message: $"Variant '{variant.Sku}' references the same image more than once.");
            }

            return null;
        }
    }
}
