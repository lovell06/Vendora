using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Catalog.Domain.Products;

public static class Errors
{
    public static readonly Error ProductNameIsNull = new()
    {
        Code = "product.name_is_null",
        Message = "Product name is requered.",
        Type = ErrorType.Validation
    };

    public static readonly Error ProductBrandIsNull = new()
    {
        Code = "product.brand_is_null",
        Message = "Product brand is requered.",
        Type = ErrorType.Validation
    };

    public static readonly Error ProductPriceIsNegative = new()
    {
        Code = "product.price_is_negative",
        Message = "Product price must be more than or equal zero.",
        Type = ErrorType.Validation
    };

    public static readonly Error InvalidProductCategory = new()
    {
        Code = "product.category_invalid",
        Message = "Category ID must not be negative.",
        Type = ErrorType.Validation
    };

    public static readonly Error SkuIsNullOrEmpty = new()
    {
        Code = "product_variant.sku_null_or_empty",
        Message = "Product variant SKU required.",
        Type = ErrorType.Validation
    };

    public static readonly Error ProductVariantNameIsNullOrEmpty = new()
    { 
        Code = "product_variant.name_is_null_or_empty",
        Message = "Product variant name required.",
        Type = ErrorType.Validation
    };

    public static readonly Error ProductVariantPriceIsNegative = new()
    {
        Code = "product_variant.price_is_negative",
        Message = "Product variant price must be positive value.",
        Type = ErrorType.Validation
    };

    public static readonly Error InvalidProductId = new()
    {
        Code = "product.invalid_id",
        Message = "Product ID is invalid.",
        Type = ErrorType.Validation
    };

    public static readonly Error ProductVariantNotFound = new()
    {
        Code = "product_variant.not_found",
        Message = "Product variant not found",
        Type = ErrorType.NotFound
    };

    public static readonly Error ProductVariantExistsBySku = new()
    {
        Code = "product_variant.exists_by_sku",
        Message = "Product variant sku must be unique.",
        Type = ErrorType.Conflict
    };

    public static readonly Error ProductImageObjectKeyEmpty = new()
    {
        Code = "product_image.object_key_empty",
        Message = "Product image object key required.",
        Type = ErrorType.Validation
    };

    public static readonly Error ProductImageContentTypeEmpty = new()
    {
        Code = "product_image.content_type_empty",
        Message = "Product image content type required.",
        Type = ErrorType.Validation
    };

    public static readonly Error InvalidProductImageIndex = new()
    {
        Code = "product_image.invalid_index",
        Message = "Product image index is invalid.",
        Type = ErrorType.Validation
    };

    public static readonly Error ProductImageNotFound = new()
    {
        Code = "product_image.not_found",
        Message = "Product image is not found.",
        Type = ErrorType.NotFound
    };

    public static readonly Error PrimaryProductImageConflict = new()
    {
        Code = "product.image_primary_conflict",
        Message = "Every product contains unique primary image.",
        Type = ErrorType.Conflict
    }; 

    public static readonly Error InvalidProductImageId = new()
    {
        Code = "product_image.invalid_id",
        Message = "Product Image ID is invalid",
        Type = ErrorType.Validation
    };

    public static readonly Error InvalidProductVariantId = new()
    {
        Code = "product_variant.invalid_id",
        Message = "Product Variant ID is invalid",
        Type = ErrorType.Validation
    };

    public static readonly Error CannotReorderImages = new()
    {
        Code = "product.cannot_reorder_images",
        Message = "Cannot reorder images.",
        Type = ErrorType.Validation
    };

    public static readonly Error ProductNotContainsImage = new()
    {
        Code = "product.not_contains_image",
        Message = "Product does not contains image.",
        Type = ErrorType.NotFound
    };

    public static readonly Error PrimaryProductVariantImageConflict = new()
    {
        Code = "product_variant.primary_image_conflict",
        Message = "Even product variant has only one image is primary",
        Type = ErrorType.Conflict
    };

    public static readonly Error InvalidProductVariantImageIndex = new()
    {
        Code = "product_variant_image.invalid_index",
        Message = "Product variant image index is invalid.",
        Type = ErrorType.Validation
    };

    public static readonly Error ImageAssignmentToVariantIndexConflict = new()
    {
        Code = "product_variant_image.index_conflict",
        Message = "Product image which assigned to variant has index exists.",
        Type = ErrorType.Conflict
    };
}