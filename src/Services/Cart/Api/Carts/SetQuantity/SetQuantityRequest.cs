using Vendora.Services.Cart.Application.Carts.SetQuantity;

namespace Vendora.Services.Cart.Api.Carts.SetQuantity;

public sealed class SetQuantityRequest
{
    public const string Pattern = "/update/{productId}";

    public int Quantity { get; init; }

    public Command ToCommand(Guid userId, long productId)
    {
        return new Command
        {
            UserId = userId,
            ProductId = productId,
            NewQuantity = Quantity
        };
    }
}
