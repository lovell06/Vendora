using Vendora.Services.Ordering.Application.Orders;

namespace Vendora.Services.Ordering.Api.Orders.List;

public sealed class ListOrdersRequest
{
    public const string Pattern = "/list";

    public int Page { get; init; }
    public int Size { get; init; }

    public ListOrders.Query ToQuery(Guid userId)
    {
        return new ListOrders.Query(userId, Page, Size);
    }
}