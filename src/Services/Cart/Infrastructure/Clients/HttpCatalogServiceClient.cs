namespace Vendora.Services.Cart.Infrastructure.Clients;

public sealed class HttpCatalogServiceClient(HttpClient client) : ICatalogServiceClient
{
    public async Task<Result> ProductExistsByIdAsync(long productId, CancellationToken ct)
    {
        using var message = new HttpRequestMessage(HttpMethod.Get, $"/api/products/{productId}");
        var response = await client.SendAsync(message, ct);

        if (response.IsSuccessStatusCode)
            return Result.Success();

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(ct);

        if (errorResponse is null)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => Result.Failure(new Error { Type = ErrorType.Unauthorized }),
                HttpStatusCode.Forbidden => Result.Failure(new Error { Type = ErrorType.Forbidden }),
                _ => throw new InvalidOperationException("Cannot parse JSON from HTTP response.")
            };
        }

        if (response.StatusCode is HttpStatusCode.InternalServerError)
            throw new InvalidOperationException("Unexpected exception from catalog service.");

        return Result.Failure(Error.FromResponse(errorResponse));
    }
}