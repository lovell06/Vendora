using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Cart.Application.Carts.Initialize;

public sealed class Command : ICommand
{
    public Guid UserId { get; init; }
}