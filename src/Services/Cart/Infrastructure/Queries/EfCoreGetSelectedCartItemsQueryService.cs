namespace Vendora.Services.Cart.Infrastructure.Queries;

public sealed class EfCoreGetSelectedCartItemsQueryService(ApplicationDbContext context) : GetSelectedCartItems.IQueryService
{
    public async Task<GetSelectedCartItems.Response?> ExecuteAsync(GetSelectedCartItems.Query query, CancellationToken ct)
    {
        var cart = await context.Carts
            .Include(c => 
                c.Items.Where(i => 
                    query.ProductVariantIds.Any(productVariantId => 
                        productVariantId == i.ProductVariantId)))
            .SingleOrDefaultAsync(c => c.UserId == query.UserId, ct);

        if (cart is null)
            return null;

        var items = cart.Items
            .Select(i => new GetSelectedCartItems.CartItemDto(i.ProductVariantId, i.Quantity ))
            .ToList();
        
        return new GetSelectedCartItems.Response(items);
    }
}