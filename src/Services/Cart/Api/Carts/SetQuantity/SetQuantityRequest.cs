using Vendora.Services.Cart.Application.Carts.SetQuantity;

namespace Vendora.Services.Cart.Api.Carts.SetQuantity;

public sealed class SetQuantityRequest
{
    public const string Pattern = "/update/{productVariantId}";

    public int Quantity { get; init; }

    public Command ToCommand(Guid userId, long productVariantId)
    {
        return new Command
        {
            UserId = userId,
            ProductVariantId = productVariantId,
            NewQuantity = Quantity
        };
    }
}
