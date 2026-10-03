namespace Vendora.Services.Cart.Application.Carts.RemoveItem;

public sealed class Command : ICommand
{
    public Guid UserId { get; init; }
    public long ProducId { get; init; }
}