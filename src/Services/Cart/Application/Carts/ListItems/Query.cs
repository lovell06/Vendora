namespace Vendora.Services.Cart.Application.Carts.ListItems;

public sealed class Query : IQuery<Response>
{
    public Guid UserId { get; init; }
}
