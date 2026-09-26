using System.Net.Http.Headers;
using System.Net.Http.Json;
using Vendora.Services.Catalog.Application.Abstractions.Authentication;
using Vendora.Services.Catalog.Application.Abstractions.Clients.Inventory;

namespace Vendora.Services.Catalog.Infrastructure.Clients;

public sealed class HttpInventoryClient(HttpClient client) : IInventoryClient
{
    public async Task<CheckAvailabilityResult> CheckAvailabilityAsync(long productId, CancellationToken cancellationToken)
    {
        var response = await client.GetAsync(
            $"/api/inventory-items/{productId}/availability",
            cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content
                .ReadAsStringAsync(cancellationToken) ?? throw new InvalidOperationException(
                    $"Has an error {response.StatusCode}");
            
            throw new InvalidOperationException(
                $"Error: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<CheckAvailabilityResult>(cancellationToken);

        return result ?? throw new InvalidOperationException(
            "Has an error: \'Content from inventory is null\'");
    }

    public async Task InitializeStockAsync(long productId, ICurrentUser currentUser, CancellationToken cancellationToken)
    {
        using var message = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/inventory-items/initialize");
        message.Content = JsonContent.Create(new { ProductId = productId });
        message.Headers.Authorization = new AuthenticationHeaderValue(
            scheme: "Bearer",
            parameter: await currentUser.GetAccessTokenAsync());

        var response = await client.SendAsync(message, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Status Code: {response.StatusCode}, Data: {await response.Content.ReadAsStringAsync(cancellationToken)}");
        }
    }
}