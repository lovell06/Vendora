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

        var count = await productQuery.CountAsync(cancellationToken);

        var products = await productQuery
            .Select(prod => new ListProducts.ProductDto(
                prod.Id,
                prod.Name,
                prod.Variants.Select(variant => variant.Price).Min(),
                prod.Variants.Select(variant => variant.Price).Max(),
                prod.Currency,
                $"{objectStorageBaseUrl.Trim('/')}/{prod.Images.Single(image => image.IsPrimary).ObjectKey}"))
            .Skip((query.Page-1) * query.Size)
            .Take(query.Size)
            .ToListAsync(cancellationToken);

        return new ListProducts.Response(
            category,
            query.Page,
            count,
            products);
    }
}