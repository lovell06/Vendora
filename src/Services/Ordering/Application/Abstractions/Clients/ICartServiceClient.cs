namespace Vendora.Services.Ordering.Application.Abstractions.Clients;

public interface ICartServiceClient
{
    Task<Result<SelectedItemsResponse>> GetSelectedItems(
        IReadOnlyList<long> productIds,
        CancellationToken ct);
}

public sealed class SelectedItemsResponse
{
    public required IReadOnlyList<CartItemDto> Items { get; init; }
}

public sealed class CartItemDto
{
    public long ProductId { get; init; }
    public int Quantity { get; init; }
}