namespace Vendora.Services.Cart.Application.Carts.GetSelectedItems;

public sealed class Query(Guid userId, List<long> productVariantIds) : IQuery<Response>
{
    public Guid UserId { get; init; } = userId;
    public List<long> ProductVariantIds { get; private set; } = [.. productVariantIds.Distinct()];
}