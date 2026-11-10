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
}