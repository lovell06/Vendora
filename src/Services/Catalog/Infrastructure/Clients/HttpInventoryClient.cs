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
}