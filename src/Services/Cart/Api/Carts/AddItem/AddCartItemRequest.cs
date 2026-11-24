namespace Vendora.Services.Cart.Api.Carts.AddItem;

public sealed class AddCartItemRequest
{
    public const string Pattern = "/add-item";
    public long ProductVariantId { get; init; }
    public int Quantity { get; init; }
}