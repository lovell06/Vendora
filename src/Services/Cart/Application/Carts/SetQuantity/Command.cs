namespace Vendora.Services.Cart.Application.Carts.SetQuantity;

public sealed class Command : ICommand
{
    public Guid UserId { get; init; }
    public long ProductVariantId { get; init; }
    public int NewQuantity { get; init; }
}