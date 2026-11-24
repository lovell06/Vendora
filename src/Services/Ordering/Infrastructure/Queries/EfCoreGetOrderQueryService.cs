namespace Vendora.Services.Ordering.Infrastructure.Queries;

public sealed class EfCoreGetOrderQueryService(ApplicationDbContext context) : GetOrder.IQueryService
{
    public async Task<GetOrder.Response?> ExecuteAsync(GetOrder.Query query, CancellationToken ct)
    {
        var order = await context.Orders
            .Where(o => o.UserId == query.UserId)
            .Where(o => o.Id == query.OrderId)
            .Include(o => o.Items)
            .Select(o => new GetOrder.OrderDetailDto(
                o.Id,
                o.Status.ToString(),
                o.Currency,
                o.TotalAmount,
                o.CreatedAt,
                o.Items.Select(i => new GetOrder.OrderItemDto(
                    i.ProductId,
                    i.ProductName,
                    i.UnitPrice,
                    i.Quantity,
                    i.SubTotal)).ToList().AsReadOnly()))
            .SingleOrDefaultAsync(ct);

        return order is null
            ? null
            : new GetOrder.Response(order);
    }
}