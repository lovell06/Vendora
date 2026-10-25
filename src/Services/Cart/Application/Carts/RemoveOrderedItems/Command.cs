namespace Vendora.Services.Cart.Application.Carts.RemoveOrderedItems;

public sealed class Command : ICommand
{
    public required Guid UserId { get; init; }
    public required IReadOnlyList<long> Items { get; init; }
}