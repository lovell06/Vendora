using Vendora.Services.Ordering.Application.Orders.List;

namespace Vendora.Services.Ordering.Api.Orders.List;

public sealed class ListOrdersRequest
{
    public const string Pattern = "/list";

    public int Page { get; init; }
    public int Size { get; init; }

    public Query ToQuery(Guid userId)
    {
        return new Query { UserId = userId, Page = Page, Size = Size };
    }
}