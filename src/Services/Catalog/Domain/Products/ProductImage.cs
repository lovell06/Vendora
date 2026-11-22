using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Catalog.Domain.Products;

public class ProductImage
{
    public long Id { get; init; }
    public required long ProductId { get; init; }
    public string ObjectKey { get; init; } = null!;
    public string ContentType { get; init; } = null!;
    public long SizeBytes { get; init; }
    public bool IsPrimary { get; private set; }
    public int SortOrder { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }

    public Product? Product { get; init; }

    private ProductImage()
    {
    }

    public static Result<ProductImage> Create(
        long productId,
        string objectKey,
        string contentType,
        long sizeBytes,
        bool isPrimary,
        int sortedOrder,
        DateTimeOffset creationDate)
    {
        if (productId < 0)
            return Result<ProductImage>.Failure(Errors.InvalidProductId);

        objectKey = objectKey.Trim();
        contentType = contentType.Trim();
        
        if (string.IsNullOrEmpty(objectKey))
            return Result<ProductImage>.Failure(Errors.ProductImageObjectKeyEmpty);
        
        if (string.IsNullOrEmpty(contentType))
            return Result<ProductImage>.Failure(Errors.ProductImageContentTypeEmpty);
        
        if (sortedOrder < 1)
            return Result<ProductImage>.Failure(Errors.InvalidProductImageIndex);
        
        return Result<ProductImage>.Success(new ProductImage
        {
            Id = 0,
            ProductId = productId,
            ObjectKey = objectKey,
            ContentType = contentType,
            IsPrimary = isPrimary,
            SizeBytes = sizeBytes,
            SortOrder = sortedOrder,
            CreatedAt = creationDate
        });
    }

    internal Result SetPrimary()
    {
        if (IsPrimary)
            return Result.Success();

        IsPrimary = true;
        return Result.Success();
    }

    internal Result UnsetPrimary()
    {
        if (!IsPrimary)
            return Result.Success();

        IsPrimary = false;
        return Result.Success();
    }

    internal Result ChangeSortOrder(int sortedOrder)
    {
        if (sortedOrder < 1)
            return Result.Failure(Errors.InvalidProductImageIndex);
            
        if (SortOrder == sortedOrder)
            return Result.Success();

        SortOrder = sortedOrder;
        return Result.Success();
    }
}