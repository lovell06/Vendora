using Vendora.Services.Cart.Application.Carts.AddItem;

namespace Vendora.Services.Cart.Api.Carts.AddItem;

public sealed class AddItemRequest
{
    public const string Pattern = "/add-item";
    public long ProductId { get; init; }
    public int Quantity { get; init; }

    public Command ToCommand(Guid userId)
    {
        return new Command
        {
            UserId = userId,
            ProductId = ProductId,
            Quantity = Quantity
        };
    }
}