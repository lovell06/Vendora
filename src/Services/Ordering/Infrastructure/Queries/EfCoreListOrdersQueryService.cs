namespace Vendora.Services.Ordering.Infrastructure.Queries;

public sealed class EfCoreListOrdersQueryService(ApplicationDbContext context) : ListOrders.IQueryService
{
    public async Task<ListOrders.Response> ExecuteAsync(ListOrders.Query query, CancellationToken ct)
    {
        var items = await context.Orders
            .AsNoTracking()
            .Where(o => o.UserId == query.UserId)
            .Select(o => new ListOrders.OrderSummaryDto(
                o.Id,
                o.Status.ToString(),
                o.Currency,
                o.Items.Sum(i => i.Quantity * i.UnitPrice),
                o.CreatedAt)).ToListAsync(ct);

        return new ListOrders.Response(
            items,
            query.Page,
            query.Size,
            items.Count);
    }
}