using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Catalog.Domain.Products;

public class ProductVariant
{
    public long Id { get; init; }
    public required string Sku { get; init; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public long ProductId { get; init; }
    public Product? Product { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private readonly List<ProductVariantImage> _imageAssignments = [];
    public IReadOnlyList<ProductVariantImage> ImageAssignments => _imageAssignments;

    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsDeleted => DeletedAt is not null;

    private ProductVariant()
    {
    }

    public static Result<ProductVariant> Create(
        string sku,
        string name,
        decimal price,
        long productId,
        DateTimeOffset createdAt)
    {
        sku = sku.Trim();
        if (string.IsNullOrEmpty(sku))
            return Result<ProductVariant>.Failure(Errors.SkuIsNullOrEmpty);
        
        if (string.IsNullOrWhiteSpace(name))
            return Result<ProductVariant>.Failure(Errors.ProductVariantNameIsNullOrEmpty);
        
        if (price < 0)
            return Result<ProductVariant>.Failure(Errors.ProductVariantPriceIsNegative);
        
        if (productId < 0)
            return Result<ProductVariant>.Failure(Errors.InvalidProductId);
        
        return Result<ProductVariant>.Success(new ProductVariant
        {
            Id = 0,
            Sku = sku,
            Name = name,
            Price = price,
            ProductId = productId,
            CreatedAt = createdAt
        });
    }

    public Result ChangeName(string name, DateTimeOffset updatedAt)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(Errors.ProductVariantNameIsNullOrEmpty);

        Name = name;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result ChangePrice(decimal price, DateTimeOffset updatedAt)
    {
        if (price < 0)
            return Result.Failure(Errors.ProductVariantPriceIsNegative);

        Price = price;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result Delete(DateTimeOffset deletedAt)
    {
        if (IsDeleted)
            return Result.Success();

        DeletedAt = deletedAt;
        UpdatedAt = deletedAt;

        return Result.Success();
    }

    internal Result AssignImage(
        ProductImage image, 
        bool isPrimary,
        DateTimeOffset updationDate)
    {                
        if (isPrimary &&
            _imageAssignments.Select(i => i.IsPrimary).Contains(true))
            return Result.Failure(Errors.PrimaryProductVariantImageConflict);

        if (_imageAssignments.Select(i => i.ProductImageId).Contains(image.Id))
            return Result.Success();

        var sortOrder = _imageAssignments.Count + 1;

        var result = ProductVariantImage.Create(
            productVariantId: Id,
            productImageId: image.Id,
            sortOrder: sortOrder,
            isPrimary: isPrimary);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        _imageAssignments.Add(result.Value);

        UpdatedAt = updationDate;

        return Result.Success();
    }

    internal Result SetPrimaryImage(long imageId, DateTimeOffset updatedAt)
    {
        var image = _imageAssignments.SingleOrDefault(i => i.ProductImageId == imageId);

        if (image is null)
            return Result.Failure(Errors.ProductImageNotFound);
        
        if (image.IsPrimary)
            return Result.Success();

        var currentPrimary = _imageAssignments.SingleOrDefault(i => i.IsPrimary);
        if (currentPrimary is not null)
        {
            var result = currentPrimary.UnsetPrimary();

            if (result.IsFailure)
                return Result.Failure(result.Error);
        }
            
        var setPrimaryResult = image.SetPrimary();
        if (setPrimaryResult.IsFailure) 
            return Result.Failure(setPrimaryResult.Error);

        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result MoveImage(long productImageId, int targetIndex, DateTimeOffset updatedAt)
    {
        var variantImage = _imageAssignments.SingleOrDefault(variantImage => variantImage.ProductImageId == productImageId);
        if (variantImage is null)
            return Result.Failure(Errors.ProductImageNotFound);
        
        if (targetIndex < 1 || targetIndex > _imageAssignments.Count)
            return Result.Failure(Errors.InvalidProductVariantImageIndex);

        var sourceIndex = variantImage.SortOrder;
        
        if (sourceIndex == targetIndex)
            return Result.Success();

        var movingBackward = sourceIndex > targetIndex;
        var startIndex = movingBackward ? targetIndex - 1 : sourceIndex;
        var endIndex = movingBackward ? sourceIndex - 1 : targetIndex;
        var offset = movingBackward ? 1 : -1;

        var orderedVariantImages = _imageAssignments.OrderBy(i => i.SortOrder).ToList();
        
        for(var index = startIndex; index < endIndex; ++index)
        {
            var currentVariantImage = orderedVariantImages[index];
            var moveResult = currentVariantImage.ChangeSortOrder(currentVariantImage.SortOrder + offset);
            if (moveResult.IsFailure)
                return Result.Failure(moveResult.Error);
        }

        var result = variantImage.ChangeSortOrder(targetIndex);
        if (result.IsFailure)
            return Result.Failure(result.Error);

        UpdatedAt = updatedAt;
        
        return Result.Success();
    }

    public Result ReorderImages(List<long> imageIds, DateTimeOffset updatedAt)
    {
        imageIds = [.. imageIds.Distinct()];

        if (imageIds.Count != _imageAssignments.Count)
            return Result.Failure(Errors.CannotReorderImages);

        var imagesById = _imageAssignments.ToDictionary(image => image.ProductImageId);

        for (var index = 0; index < imageIds.Count; ++index)
        {
            var newIndex = index + 1;
            if (!imagesById.TryGetValue(imageIds[index], out var image))
                return Result.Failure(Errors.ProductImageNotFound);
            
            if (image.SortOrder == newIndex)
                continue;

            var result = image.ChangeSortOrder(newIndex);
            if (result.IsFailure)
                return Result.Failure(result.Error);
        }

        UpdatedAt = updatedAt;

        return Result.Success();
    }
}