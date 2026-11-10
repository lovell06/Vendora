namespace Vendora.Services.Ordering.Application.Abstractions.Clients;

public interface ICartServiceClient
{
    Task<Result<SelectedItemsResponse>> GetSelectedItems(
        IReadOnlyList<long> productVariantIds,
        CancellationToken ct);
}

public sealed class SelectedItemsResponse
{
    public required IReadOnlyList<CartItemDto> Items { get; init; }
}

public struct CartItemDto
{
    public long ProductVariantId { get; init; }
    public int Quantity { get; init; }
}