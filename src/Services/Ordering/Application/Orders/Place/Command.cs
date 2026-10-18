namespace Vendora.Services.Ordering.Application.Orders.Place;

public sealed class Command : ICommand
{
    public Guid UserId { get; init; }
    public required IReadOnlyList<long> ProductIds { get; init; }
}
