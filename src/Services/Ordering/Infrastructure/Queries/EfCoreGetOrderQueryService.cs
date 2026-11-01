using GetOrderQuery = Vendora.Services.Ordering.Application.Orders.Get.Query;
using GetOrderResponse = Vendora.Services.Ordering.Application.Orders.Get.Response;

namespace Vendora.Services.Ordering.Infrastructure.Queries;

public sealed class EfCoreGetOrderQueryService(ApplicationDbContext context) : IGetOrderQueryService
{
    public async Task<GetOrderResponse?> ExecuteAsync(GetOrderQuery query, CancellationToken ct)
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
            : new GetOrderResponse { Order = order };
    }
}