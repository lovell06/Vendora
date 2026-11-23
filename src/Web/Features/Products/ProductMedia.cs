namespace Vendora.Web.Features.Products;

// Presentation models. Variants reference images owned by the product gallery.
public sealed record ProductImage(string Id, string Url, string Alt);

public sealed record ProductVariant(
    string Id, string Name, string ProductCode, IReadOnlyList<string> ImageIds,
    string PrimaryImageId, bool InStock = true);
