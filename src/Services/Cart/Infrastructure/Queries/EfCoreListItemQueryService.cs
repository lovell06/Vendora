using Microsoft.EntityFrameworkCore;
using Vendora.Services.Cart.Application.Carts.ListItems;
using Vendora.Services.Cart.Infrastructure.Persistence;

namespace Vendora.Services.Cart.Infrastructure.Queries;

public sealed class EfCoreListItemQueryService(PostgresDbContext context) : IListItemsQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken ct)
    {
        var cartId = await context.Carts
            .Where(c => c.UserId == query.UserId)
            .Select(c => c.Id)
            .SingleOrDefaultAsync(ct);

        if (cartId == 0)
            return null;
        
        var items = await context.CartItems
            .Where(i => i.CartId == cartId)
            .Select(i => new ItemDto()
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            })
            .ToListAsync(ct);

        return new Response { Items = items };
    }
}