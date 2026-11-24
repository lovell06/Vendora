namespace Vendora.Services.Cart.Api.Carts.GetSelectedItems;

public sealed class GetSelectedCartItemsRequest
{
    public const string Pattern = "/items/query";
    public required List<long> ProductVariantIds { get; init; }
}