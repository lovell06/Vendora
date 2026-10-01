using System.Net.Http.Json;
using Vendora.Services.Identity.Application.Abstractions.Clients.Cart;

namespace Vendora.Services.Identity.Infrastructure.Clients;

public sealed class HttpCartClient(HttpClient client) : ICartClient
{
    public async Task InitializeCartAsync(Guid userId, CancellationToken ct)
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, "/api/carts/initialize");
        message.Content = JsonContent.Create(new { UserId = userId });

        var response = await client.SendAsync(message, ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(response.Content.ToString());
        }
    }
}
