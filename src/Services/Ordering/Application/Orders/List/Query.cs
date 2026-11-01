namespace Vendora.Services.Ordering.Application.Orders.List;

public sealed class Query : IQuery<Response>
{
    public required Guid UserId { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
}