namespace Vendora.Services.Cart.Application.Carts.AddItem;

public sealed class Command : ICommand
{
    public Guid UserId { get; init; }
    public long ProductVariantId { get; init; }
    public int Quantity { get; init; }
}