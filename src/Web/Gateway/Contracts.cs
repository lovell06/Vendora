namespace Vendora.Web.Gateway;

// Response shapes of the backend APIs, as exposed through the API Gateway.

public sealed record CategoryDto(int Id, string Name);

public sealed record CategoryListDto(int Page, int TotalCount, IReadOnlyList<CategoryDto> Categories);

public sealed record ProductSummaryDto(
    long Id, string Name, decimal MinPrice, decimal MaxPrice, string Currency, string ImageUrl);

public sealed record ProductListDto(
    CategoryDto? Category, int Page, int TotalCount, IReadOnlyList<ProductSummaryDto> Products);

public sealed record ProductImageDto(long Id, string Url, int SortOrder);

public sealed record ProductVariantDto(
    long Id, string Sku, string Name, decimal Price, IReadOnlyList<long> ImageIds,
    bool InStock, int AvailableQuantity);

public sealed record ProductDetailDto(
    long Id, string Name, string? Description, string Brand, CategoryDto Category,
    string Currency, string Status,
    IReadOnlyList<ProductVariantDto> Variants, IReadOnlyList<ProductImageDto> Images);

public sealed record ProductDetailResponse(ProductDetailDto Product);

public sealed record VariantLookupDto(
    long ProductId, long VariantId, string ProductName, string VariantName,
    string Sku, decimal Price, string Currency);

public sealed record VariantLookupResponse(IReadOnlyList<VariantLookupDto> ProductVariants);

public sealed record CartItemDto(long ProductVariantId, int Quantity);

public sealed record CartResponse(IReadOnlyList<CartItemDto> Items);

public sealed record LoginResponse(Guid UserId, string AccessToken, string RefreshToken);
