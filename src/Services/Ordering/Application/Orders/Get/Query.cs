namespace Vendora.Services.Ordering.Application.Orders.Get;

public sealed class Query : IQuery<Response>
{
    public required Guid UserId { get; init; }
    public required long OrderId { get; init; }
}