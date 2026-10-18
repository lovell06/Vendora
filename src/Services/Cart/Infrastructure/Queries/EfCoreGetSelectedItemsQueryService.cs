using Query = Vendora.Services.Cart.Application.Carts.GetSelectedItems.Query;
using Response = Vendora.Services.Cart.Application.Carts.GetSelectedItems.Response;

namespace Vendora.Services.Cart.Infrastructure.Queries;

public sealed class EfCoreGetSelectedItemsQueryService(ApplicationDbContext context) : IGetSelectedItemsQueryService
{
    public async Task<Response?> ExecuteAsync(Query query, CancellationToken ct)
    {
        var cart = await context.Carts
            .Include(c => 
                c.Items.Where(i => 
                    query.ProductIds.Any(productId => 
                        productId == i.ProductId)))
            .SingleOrDefaultAsync(c => c.UserId == query.UserId, ct);

        if (cart is null)
            return null;

        var items = cart.Items
            .Select(i => new CartItemDto { ProductId = i.ProductId, Quantity = i.Quantity })
            .ToList();
        
        return new Response { Items = items };
    }
}