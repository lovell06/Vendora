namespace Vendora.Services.Cart.Api.Carts.SetQuantity;

public sealed class SetCartItemQuantityRequest
{
    public const string Pattern = "/update/{productVariantId}";
    public int Quantity { get; init; }
}
