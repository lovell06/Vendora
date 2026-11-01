using Vendora.Services.Ordering.Application.Orders.Get;

namespace Vendora.Services.Ordering.Infrastructure.Queries;

public sealed class EfCoreGetOrderQueryService(ApplicationDbContext context) : IQueryService
{
    public async Task<Response?> GetOrderAsync(Query query, CancellationToken ct)
    {
        var order = await context.Orders
            .Where(o => o.UserId == query.UserId)
            .Include(o => o.Items)
            .Select(o => new OrderDetailDto
            {
                Id = o.Id,
                Status = o.Status.ToString(),
                Currency = o.Currency,
                TotalAmount = o.TotalAmount,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    LineTotal = i.SubTotal
                }).ToList().AsReadOnly(),
                CreatedAt = o.CreatedAt
            })
            .SingleOrDefaultAsync(o => o.Id == query.OrderId, ct);

        return order is null
            ? null
            : new Response {Order = order};
    }
}