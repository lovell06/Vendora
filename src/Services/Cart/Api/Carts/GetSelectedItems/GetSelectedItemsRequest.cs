using Vendora.Services.Cart.Application.Carts.GetSelectedItems;

namespace Vendora.Services.Cart.Api.Carts.GetSelectedItems;

public sealed class GetSelectedItemsRequest
{
    public const string Pattern = "/items/query";

    public required List<long> ProductIds { get; init; }

    public Query ToQuery(Guid userId)
    {
        return new Query(userId, ProductIds);
    }
}