using Vendora.Services.Catalog.Application.Products.Get;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class EfCoreGetProductQueryService(ApplicationDbContext context, IConfiguration configuration) : IGetProductQueryService
{
    public async Task<ProductDto?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        var publicBaseUrl = configuration["AzureBlobStorage:PublicBaseUrl"];
        if (publicBaseUrl is null)
            throw new InvalidOperationException("Missing AzureBlobStorage:PublicBaseUrl configuration.");

        publicBaseUrl = publicBaseUrl.Trim('/');
        
        return await context.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Brand = product.Brand,
                Category = new CategoryDto
                {
                    Id = product.Category!.Id,
                    Name = product.Category.Name
                },
                Currency = product.Currency,
                Status = product.Status.ToString(),
                Variants = product.Variants.Select(variant => new ProductVariantDto
                {
                    Name = variant.Name,
                    Sku = variant.Sku,
                    Price = variant.Price,
                    ImageIds = variant.ImageAssignments.Select(image => image.ProductImageId).ToList()
                }).ToList(),
                Images = product.Images.Select(image => new ProductImageDto
                {
                    Id = image.Id,
                    SortOrder = image.SortOrder,
                    Url = $"{publicBaseUrl}/{image.ObjectKey.Trim('/')}"
                }).ToList()
            })
            .SingleOrDefaultAsync(product => product.Id == query.Id, cancellationToken);
    }
}