namespace Vendora.Services.Identity.Application.Abstractions.Clients.Cart;

public interface ICartClient
{
    Task InitializeCartAsync(Guid userId, CancellationToken ct);
}
