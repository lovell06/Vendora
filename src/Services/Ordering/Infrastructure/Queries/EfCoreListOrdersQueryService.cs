using ListOrdersQuery = Vendora.Services.Ordering.Application.Orders.List.Query;
using ListOrdersResponse = Vendora.Services.Ordering.Application.Orders.List.Response;

namespace Vendora.Services.Ordering.Infrastructure.Queries;

public sealed class EfCoreListOrdersQueryService(ApplicationDbContext context) : IListOrdersQueryService
{
    public async Task<ListOrdersResponse> ExecuteAsync(ListOrdersQuery query, CancellationToken ct)
    {
        var items = await context.Orders
            .AsNoTracking()
            .Where(o => o.UserId == query.UserId)
            .Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                Status = o.Status.ToString(),
                Currency = o.Currency,
                TotalAmount = o.Items.Sum(i => i.Quantity * i.UnitPrice),
                CreatedAt = o.CreatedAt
            }).ToListAsync(ct);

        return new ListOrdersResponse
        {
            Items = items,
            Page = query.Page,
            Size = query.Size,
            TotalCount = items.Count
        };
    }
}