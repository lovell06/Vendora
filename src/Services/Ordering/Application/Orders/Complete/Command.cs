namespace Vendora.Services.Ordering.Application.Orders.Complete;

public sealed class Command :ICommand
{
    public required long OrderId { get; init; }
}