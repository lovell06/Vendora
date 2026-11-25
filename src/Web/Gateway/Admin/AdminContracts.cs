namespace Vendora.Web.Gateway.Admin;

public sealed record ProductVariantInput(string Sku, string Name, decimal Price, IReadOnlyList<int> ImageIndexes);

public sealed record ProductImageFile(string FileName, string ContentType, Func<Stream> OpenRead);

public sealed record CreateProductInput(
    string Name, string? Description, string Brand, int CategoryId, bool IsVisible,
    IReadOnlyList<ProductVariantInput> Variants, IReadOnlyList<ProductImageFile> Images);

public sealed record CreatedProductDto(long Id);
