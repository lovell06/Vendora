namespace Vendora.Services.Ordering.Application.Orders.Deliver;

public sealed class Command : ICommand
{
    public required long OrderId { get; init; }
}