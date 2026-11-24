namespace Vendora.Services.Cart.Api.Carts.RemoveItem;

public sealed class RemoveCartItemRequest
{
    public const string Pattern = "/remove-item/{productVariantId}";
    public long ProductVariantId { get; init; }
}