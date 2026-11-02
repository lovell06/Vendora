namespace Vendora.Services.Ordering.Application.Orders.Cancel;

public sealed class Command : ICommand
{
    public Guid UserId { get; init; }
    public long OrderId { get; init; }
}