namespace Vendora.Services.Cart.Application.Carts.GetSelectedItems;

public interface IGetSelectedItemsQueryService
{
    Task<Response?> ExecuteAsync(Query query, CancellationToken ct);
}