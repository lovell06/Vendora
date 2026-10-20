namespace Vendora.Services.Ordering.Infrastructure.Clients;

public sealed class HttpCatalogServiceClient(
    HttpClient client) : ICatalogServiceClient
{
    public async Task<Result<ProductsInfoResponse>> GetProductsInfoAsync(IReadOnlyList<long> productIds, CancellationToken ct)
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, "/api/products/batch");
        message.Content = JsonContent.Create(new { ProductIds = productIds });

        using var response = await client.SendAsync(message, ct);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode is HttpStatusCode.Unauthorized)
            {
                return Result<ProductsInfoResponse>.Failure(new Error
                {
                    Type = ErrorType.Unauthorized
                });
            }

            if (response.StatusCode is HttpStatusCode.Forbidden)
            {
                return Result<ProductsInfoResponse>.Failure(new Error
                {
                    Type = ErrorType.Forbidden
                });
            }
            
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(ct)
                                ?? throw new InvalidOperationException("Cannot convert JSON from HTTP Response.");
            return Result<ProductsInfoResponse>.Failure(Error.FromResponse(errorResponse));
        }

        var content = await response.Content.ReadFromJsonAsync<ProductsInfoResponse>(ct);
        return content is null
            ? throw new InvalidOperationException("Cannot convert JSON from HTTP response.")
            : Result<ProductsInfoResponse>.Success(content);
    }
}