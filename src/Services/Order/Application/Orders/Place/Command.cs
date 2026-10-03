namespace Vendora.Services.Order.Application.Orders.Place;

public sealed class Command : ICommand
{
    public Guid UserId { get; init; }
    public required List<long> ProductIds { get; init; }
}
