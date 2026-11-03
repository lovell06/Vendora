namespace Vendora.Services.Ordering.Application.Orders.Prepare;

public sealed class Command : ICommand
{
    public required long OrderId { get; init; }
}