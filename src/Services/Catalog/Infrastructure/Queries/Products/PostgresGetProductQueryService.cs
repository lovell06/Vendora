using Microsoft.EntityFrameworkCore;
using Vendora.Services.Catalog.Application.Products.Get;
using Vendora.Services.Catalog.Infrastructure.Persistence;

namespace Vendora.Services.Catalog.Infrastructure.Queries.Products;

public sealed class PostgresGetProductQueryService(PostgresDbContext context) : IGetProductQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken cancellationToken)
    {
        return await context.Products
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
                Currency = product.Currency,
                Status = product.Status.ToString()
            })
            .SingleOrDefaultAsync(product => product.Id == query.Id, cancellationToken);
    }
}