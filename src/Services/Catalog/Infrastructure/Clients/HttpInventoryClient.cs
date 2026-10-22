using System.Net;

namespace Vendora.Services.Catalog.Infrastructure.Clients;

public sealed class HttpInventoryClient(HttpClient client) : IInventoryClient
{
    public async Task<Result<CheckAvailabilityResult>> CheckAvailabilityAsync(long productId, CancellationToken cancellationToken)
    {
        var response = await client.GetAsync(
            $"/api/inventory-items/{productId}/availability",
            cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken);

            if (errorResponse is null)
            {
                return response.StatusCode switch
                {
                    HttpStatusCode.Forbidden => Result<CheckAvailabilityResult>.Failure(new Error
                    {
                        Type = ErrorType.Forbidden
                    }),
                    HttpStatusCode.Unauthorized => Result<CheckAvailabilityResult>.Failure(new Error
                    {
                        Type = ErrorType.Unauthorized
                    }),
                    _ => Result<CheckAvailabilityResult>.Failure(new Error() { Type = ErrorType.Failure })
                };
            }
            
            return Result<CheckAvailabilityResult>.Failure(Error.FromResponse(errorResponse));
        }

        var result = await response.Content.ReadFromJsonAsync<CheckAvailabilityResult>(cancellationToken);

        return result is null
            ? throw new InvalidOperationException("Has an error: \'Content from inventory is null\'")
            : Result<CheckAvailabilityResult>.Success(result);
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