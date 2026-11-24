namespace Vendora.Services.Cart.Infrastructure.Queries;

public sealed class EfCoreListCartItemQueryService(ApplicationDbContext context) : ListCartItems.IQueryService
{
    public async Task<ListCartItems.Response?> ExecuteAsync(ListCartItems.Query query, CancellationToken ct)
    {
        var cartId = await context.Carts
            .Where(c => c.UserId == query.UserId)
            .Select(c => c.Id)
            .SingleOrDefaultAsync(ct);

        if (cartId == 0)
            return null;
        
        var items = await context.CartItems
            .Where(i => i.CartId == cartId)
            .Select(i => new ListCartItems.ItemDto(i.ProductVariantId, i.Quantity))
            .ToListAsync(ct);

        return new ListCartItems.Response(items);
    }
}