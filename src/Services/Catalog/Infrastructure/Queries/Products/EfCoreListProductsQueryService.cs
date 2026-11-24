namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class EfCoreListProductsQueryService(ApplicationDbContext context, IConfiguration configuration) : ListProducts.IQueryService
{
    public async Task<ListProducts.Response?> ExecuteAsync(ListProducts.Query query, CancellationToken cancellationToken)
    {
        var objectStorageBaseUrl = configuration["AzureBlobStorage:PublicBaseUrl"]
                                   ?? throw new InvalidOperationException(
                                       "Missing AzureBlobStorage:PublicBaseUrl configuration.");
        
        ListProducts.CategoryDto? category = null;

        if (query.CategoryId is not null)
        {
            category = await context.Categories
                .Where(cat => cat.Id == query.CategoryId)
                .Select(cat => new ListProducts.CategoryDto(
                    cat.Id,
                    cat.Name))
                .SingleOrDefaultAsync(cancellationToken);

            if (category is null)
                return null;
        }

        var productQuery = context.Products
            .AsNoTracking()
            .OrderByDescending(prod => prod.CreatedAt)
            .ThenByDescending(prod => prod.Id)
            .Where(prod => prod.IsVisible && prod.DeletedAt == null);

        if (query.CategoryId is not null)
        {
            productQuery = productQuery
                .Where(prod => prod.CategoryId == query.CategoryId);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim().ToLowerInvariant();
            productQuery = productQuery
                .Where(prod => prod.Name.ToLower().Contains(term));
        }

        var count = await productQuery.CountAsync(cancellationToken);

        // A product may have no primary image (or no image at all): fall back to the
        // first image by sort order, else leave ImageUrl null. The object key is
        // projected and the URL built after materializing, because EF cannot translate
        // the null handling inside the interpolated string.
        var rows = await productQuery
            .Select(prod => new
            {
                prod.Id,
                prod.Name,
                MinPrice = prod.Variants.Select(variant => (decimal?)variant.Price).Min() ?? 0m,
                MaxPrice = prod.Variants.Select(variant => (decimal?)variant.Price).Max() ?? 0m,
                prod.Currency,
                ImageObjectKey = prod.Images
                    .OrderByDescending(image => image.IsPrimary)
                    .ThenBy(image => image.SortOrder)
                    .Select(image => image.ObjectKey)
                    .FirstOrDefault()
            })
            .Skip((query.Page-1) * query.Size)
            .Take(query.Size)
            .ToListAsync(cancellationToken);

        var baseUrl = objectStorageBaseUrl.Trim('/');
        var products = rows
            .Select(row => new ListProducts.ProductDto(
                row.Id,
                row.Name,
                row.MinPrice,
                row.MaxPrice,
                row.Currency,
                row.ImageObjectKey is null ? null : $"{baseUrl}/{row.ImageObjectKey.Trim('/')}"))
            .ToList();

        return new ListProducts.Response(
            category,
            query.Page,
            count,
            products);
    }
}