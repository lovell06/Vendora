namespace Vendora.Services.Cart.Application.Carts.GetSelectedItems;

public sealed class Query(Guid userId, List<long> productIds) : IQuery<Response>
{
    public Guid UserId { get; init; } = userId;
    public List<long> ProductIds { get; private set; } = [.. productIds.Distinct()];
}