using Vendora.BuildingBlocks.Results;
using Vendora.Services.Catalog.Domain.Categories;

namespace Vendora.Services.Catalog.Domain.Products;

public class Product
{
    public long Id { get; init; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Brand { get; private set; } = null!;
    public int CategoryId { get; private set; }
    public Category? Category { get; private set; } = null;
    public string Currency { get; private set; } = null!;
    public ProductStatus Status { get; private set; }
    public bool IsVisible { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsDeleted => DeletedAt is not null;

    private readonly List<ProductVariant> _variants = [];
    public IReadOnlyList<ProductVariant> Variants => _variants;

    private readonly List<ProductImage> _images = [];
    public IReadOnlyList<ProductImage> Images => _images;

    private Product()
    {
    }

    public static Result<Product> Create(
        string name,
        string? description,
        string brand,
        int categoryId,
        string currency,
        bool isVisible,
        DateTimeOffset createdAt)
    {
        name = name.Trim();
        description = description?.Trim();
        brand = brand.Trim();

        if (string.IsNullOrEmpty(name))
            return Result<Product>.Failure(Errors.ProductNameIsNull);

        if (string.IsNullOrEmpty(brand))
            return Result<Product>.Failure(Errors.ProductBrandIsNull);

        if (categoryId < 0)
            return Result<Product>.Failure(Errors.InvalidProductCategory);

        return Result<Product>.Success(new Product
        {
            Id = 0,
            Name = name,
            Description = description,
            Brand = brand,
            CategoryId = categoryId,
            Currency = currency,
            Status = ProductStatus.Active,
            IsVisible = isVisible,
            CreatedAt = createdAt,
            UpdatedAt = null,
            DeletedAt = null
        });
    }

    public Result Rename(string name, DateTimeOffset updatedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_rename.deleted",
                Message = "Cannot rename a deleted product.",
                Type = ErrorType.Validation
            });
        }
        
        Name = name;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result ChangeDescription(string description, DateTimeOffset updatedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_change_description.deleted",
                Message = "Cannot change description of a deleted product.",
                Type = ErrorType.Validation
            });
        }

        Description = description;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result ChangeBrand(string brandName, DateTimeOffset updatedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_change_brand.deleted",
                Message = "Cannot change brand of a deleted product.",
                Type = ErrorType.Validation
            });
        }

        Brand = brandName;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result ChangeCategory(int categoryId, DateTimeOffset updatedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_change_category.deleted",
                Message = "Cannot change category of a deleted product.",
                Type = ErrorType.Validation
            });
        }

        CategoryId = categoryId;
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result Discontinue(DateTimeOffset discontinuedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_discontinue.deleted",
                Message = "Cannot discontinue a deleted product.",
                Type = ErrorType.Validation
            });
        }

        if (Status is ProductStatus.Discontinued)
            return Result.Success();

        Status = ProductStatus.Discontinued;
        UpdatedAt = discontinuedAt;

        return Result.Success();
    }

    public Result Reactivate(DateTimeOffset reactivedAt)
    {
        if (IsDeleted)
        {
            return Result.Failure(new Error
            {
                Code = "product_continue.deleted",
                Message = "Cannot reactivate a deleted product.",
                Type = ErrorType.Validation
            });
        }

        if (Status is ProductStatus.Active)
            return Result.Success();

        Status = ProductStatus.Active;
        UpdatedAt = reactivedAt;

        return Result.Success();
    }

    public void Hide(DateTimeOffset updatedAt)
    {
        if (!IsVisible)
            return;
        
        IsVisible = false;
        UpdatedAt = updatedAt;
    }

    public void Show(DateTimeOffset updatedAt)
    {
        if (IsVisible)
            return;

        IsVisible = true;
        UpdatedAt = updatedAt;
    }

    public void Delete(DateTimeOffset deletedAt)
    {
        if (IsDeleted)
            return;

        DeletedAt = deletedAt;
        UpdatedAt = deletedAt;
    }

    public void Restore(DateTimeOffset restoredAt)
    {
        if (!IsDeleted)
            return;

        DeletedAt = null;
        UpdatedAt = restoredAt;
    }

    public Result AddVariant(ProductVariant variant, DateTimeOffset updatedAt)
    {
        if (_variants.Any(v => v.Sku == variant.Sku))
            return Result.Failure(Errors.ProductVariantExistsBySku);
        
        _variants.Add(variant);
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result RemoveVariant(string sku, DateTimeOffset updatedAt)
    {
        var variant = _variants.SingleOrDefault(v => v.Sku == sku);

        if (variant is null)
            return Result.Success();

        variant.Delete(updatedAt);
        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result AddImage(ProductImage image, DateTimeOffset updatedAt)
    {
        if (image.IsPrimary &&
            _images.Any(i => i.IsPrimary))
            return Result.Failure(Errors.PrimaryProductImageConflict);

        _images.Add(image);

        return Result.Success();
    }

    public Result MoveImage(long imageId, int targetIndex, DateTimeOffset updatedAt)
    {
        var image = _images.SingleOrDefault(image => image.Id == imageId);
        if (image is null)
            return Result.Failure(Errors.ProductImageNotFound);
        
        if (targetIndex < 1 || targetIndex > _images.Count)
            return Result.Failure(Errors.InvalidProductImageIndex);

        var sourceIndex = image.SortOrder;
        
        if (sourceIndex == targetIndex)
            return Result.Success();

        var movingBackward = sourceIndex > targetIndex;
        var startIndex = movingBackward ? targetIndex - 1 : sourceIndex;
        var endIndex = movingBackward ? sourceIndex - 1 : targetIndex;
        var offset = movingBackward ? 1 : -1;

        var orderedImages = _images.OrderBy(i => i.SortOrder).ToList();
        
        for(var index = startIndex; index < endIndex; ++index)
        {
            var currentImage = orderedImages[index];
            var moveResult = currentImage.ChangeSortOrder(currentImage.SortOrder + offset);
            if (moveResult.IsFailure)
                return Result.Failure(moveResult.Error);
        }

        var result = image.ChangeSortOrder(targetIndex);
        if (result.IsFailure)
            return Result.Failure(result.Error);

        UpdatedAt = updatedAt;
        
        return Result.Success();
    }

    public Result ReorderImages(List<long> orderedImageIds, DateTimeOffset updatedAt)
    {
        orderedImageIds = [.. orderedImageIds.Distinct()];

        if (orderedImageIds.Count != _images.Count)
            return Result.Failure(Errors.CannotReorderImages);

        var imagesById = _images.ToDictionary(image => image.Id);

        for (var index = 0; index < orderedImageIds.Count; ++index)
        {
            if (!imagesById.TryGetValue(orderedImageIds[index], out var image))
                return Result.Failure(Errors.ProductImageNotFound);

            var result = image.ChangeSortOrder(index + 1);

            if (result.IsFailure)
                return Result.Failure(result.Error);
        }

        UpdatedAt = updatedAt;

        return Result.Success();
    }

    public Result SetPrimaryImage(long imageId)
    {
        var newPrimaryImage = _images.SingleOrDefault(image => image.Id == imageId);
        if (newPrimaryImage is null)
            return Result.Failure(Errors.ProductImageNotFound);
        
        if (newPrimaryImage.IsPrimary)
            return Result.Success();

        var unsetPrimaryImageResult = _images.Single(image => image.IsPrimary).UnsetPrimary();
        if (unsetPrimaryImageResult.IsFailure)
            return Result.Failure(unsetPrimaryImageResult.Error);

        var setNewPrimaryResult = newPrimaryImage.SetPrimary();

        return setNewPrimaryResult.IsFailure
            ? Result.Failure(setNewPrimaryResult.Error)
            : Result.Success();
    }

    public Result AssignImageToVariant(
        ProductImage image, 
        ProductVariant variant, 
        bool isPrimary,
        DateTimeOffset updatedAt)
    {
        var assignImageResult = variant.AssignImage(image, isPrimary, updatedAt);

        if (assignImageResult.IsFailure)
            return Result.Failure(assignImageResult.Error);

        UpdatedAt = updatedAt;

        return Result.Success();
    }
}