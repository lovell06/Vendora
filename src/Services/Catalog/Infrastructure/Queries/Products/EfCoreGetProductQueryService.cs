using Vendora.Services.Catalog.Application.Products.Get;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class EfCoreGetProductQueryService(
    ApplicationDbContext context,
    IInventoryClient inventoryClient) : IGetProductQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        var availability = await inventoryClient.CheckAvailabilityAsync(query.Id, cancellationToken);

        return await context.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .Select(product => new Response
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
                Price = product.Price,
                IsAvailable = availability.IsAvailable,
                AvailableQuantity = availability.AvailableQuantity,
                Currency = product.Currency,
                Status = product.Status.ToString()
            })
            .SingleOrDefaultAsync(product => product.Id == query.Id, cancellationToken);
    }
}