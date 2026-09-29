using Vendora.Services.Cart.Application.Carts.RemoveItem;

namespace Vendora.Services.Cart.Api.Carts.RemoveItem;

public sealed class RemoveItemRequest
{
    public const string Pattern = "/remove-item/{productId}";
    public long ProductId { get; init; }

    public Command ToCommand(Guid userId)
    {
        return new Command
        {
            UserId = userId,
            ProducId = ProductId
        };
    }
}