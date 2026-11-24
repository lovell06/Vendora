namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class EfCoreGetProductQueryService(ApplicationDbContext context, IConfiguration configuration) : GetProduct.IQueryService
{
    public async Task<GetProduct.ProductDto?> ExecuteAsync(GetProduct.Query query, CancellationToken cancellationToken)
    {
        var publicBaseUrl = configuration["AzureBlobStorage:PublicBaseUrl"];
        if (publicBaseUrl is null)
            throw new InvalidOperationException("Missing AzureBlobStorage:PublicBaseUrl configuration.");

        publicBaseUrl = publicBaseUrl.Trim('/');
        
        return await context.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .Where(product => product.Id == query.Id)
            .Select(product => new GetProduct.ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Brand,
                new GetProduct.CategoryDto(
                    product.Category!.Id,
                    product.Category.Name),
                product.Currency,
                product.Status.ToString(),
                product.Variants.Select(variant => new GetProduct.ProductVariantDto(
                    variant.Id,
                    variant.Sku,
                    variant.Name,
                    variant.Price,
                    variant.ImageAssignments.Select(image => image.ProductImageId).ToList(),
                    false,
                    0)).ToList(),
                product.Images.Select(image => new GetProduct.ProductImageDto(
                    image.Id,
                    $"{publicBaseUrl}/{image.ObjectKey.Trim('/')}",
                    image.SortOrder)).ToList()))
            .SingleOrDefaultAsync(cancellationToken);
    }
}