using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Cart.Application.Carts.SetQuantity;

public sealed class Command : ICommand
{
    public Guid UserId { get; init; }
    public long ProductId { get; init; }
    public int NewQuantity { get; init; }
}