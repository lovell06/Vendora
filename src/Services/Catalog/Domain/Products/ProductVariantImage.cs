using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Catalog.Domain.Products;

public class ProductVariantImage
{
    public long Id { get; init; }
    public long ProductImageId { get; init; }
    public ProductImage? ProductImage { get; init; }
    public long ProductVariantId { get; init; }
    public ProductVariant? ProductVariant { get; init; }
    public bool IsPrimary { get; private set; }
    public int SortOrder { get; private set; }

    private ProductVariantImage()
    {
    }

    public static Result<ProductVariantImage> Create(
        long productImageId,
        long productVariantId,
        int sortOrder,
        bool isPrimary)
    {
        if (productImageId < 0)
            return Result<ProductVariantImage>.Failure(Errors.InvalidProductImageId);

        if (productVariantId < 0)
            return Result<ProductVariantImage>.Failure(Errors.InvalidProductVariantId);
        
        if (sortOrder < 1)
            return Result<ProductVariantImage>.Failure(Errors.InvalidProductVariantImageIndex);
        
        return Result<ProductVariantImage>.Success(new ProductVariantImage
        {
            Id = 0,
            ProductImageId = productImageId,
            ProductVariantId = productVariantId,
            SortOrder = sortOrder,
            IsPrimary = isPrimary
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

    internal Result ChangeSortOrder(int sortOrder)
    {
        if (sortOrder == SortOrder)
            return Result.Success();

        SortOrder = sortOrder;

        return Result.Success();
    }
}