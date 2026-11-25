namespace Vendora.Services.Inventory.Application.InventoryItems;

public static class ListInventoryItems
{
    public sealed record Query(int Page, int Size) : IQuery<Response>;

    public sealed record Response(IReadOnlyList<InventoryItemDto> InventoryItems, int Page, int TotalCount);

    public sealed record InventoryItemDto(
        long Id,
        long ProductId,
        long ProductVariantId,
        string Sku,
        int OnHandQuantity,
        int ReservedQuantity);

    public interface IQueryService
    {
        Task<Response> ExecuteAsync(Query query, CancellationToken cancellationToken);
    }

    public sealed class Handler(IQueryService queryService) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            return Result<Response>.Success(
                await queryService.ExecuteAsync(query, cancellationToken));
        }
    }
}
