namespace Vendora.Services.Cart.Application.Carts.ListItems;

public interface IListItemsQueryService
{
    Task<Response?> ExecuteAsync(Query query, CancellationToken ct);
}
