using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Application.Products.List;
using Vendora.Services.Catalog.Infrastructure.Persistence;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class PostgresListProductsQueryService(PostgresDbContext context) : IListProductsQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        CategoryDto? category = null;

        if (query.CategoryId is not null)
        {
            category = await context.Categories
                .Select(cat => new CategoryDto
                {
                    Id = cat.Id,
                    Name = cat.Name
                })
                .SingleOrDefaultAsync(cat => cat.Id == query.CategoryId, cancellationToken);

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
            .Select(prod => new ProductDto
            {
                Id = prod.Id,
                Name = prod.Name,
                Currency = prod.Currency,
                Price = prod.Price
            })
            .Skip((query.Page-1) * query.Size)
            .Take(query.Size)
            .ToListAsync(cancellationToken);

        return new Response
        {
            Category = category,
            Page = query.Page,
            TotalCount = count,
            Products = products
        };
    }
}