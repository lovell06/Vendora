using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Ordering.Infrastructure.Clients;

public sealed class HttpCartServiceClient(
    HttpClient client,
    ICurrentUser user) : ICartServiceClient
{
    public async Task<Result<SelectedItemsResponse>> GetSelectedItems(IReadOnlyList<long> productIds, CancellationToken ct)
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, "/api/carts/items/query");
        message.Headers.Authorization = new AuthenticationHeaderValue(
            scheme: "bearer",
            parameter: await user.GetAccessTokenAsync());
        message.Content = JsonContent.Create(new { ProductIds = productIds });

        using var response = await client.SendAsync(message, ct);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode is HttpStatusCode.Unauthorized)
            {
                return Result<SelectedItemsResponse>.Failure(new Error
                {
                    Type = ErrorType.Unauthorized
                });
            }

            if (response.StatusCode is HttpStatusCode.Forbidden)
            {
                return Result<SelectedItemsResponse>.Failure(new Error
                {
                    Type = ErrorType.Forbidden
                });
            }
            
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(ct)
                                ?? throw new InvalidOperationException("Cannot convert JSON from HTTP Response.");
            return Result<SelectedItemsResponse>.Failure(Error.FromResponse(errorResponse));
        }

        var content = await response.Content.ReadFromJsonAsync<SelectedItemsResponse>(ct)
                           ?? throw new InvalidOperationException("Cannot convert JSON from HTTP Response.");

        return Result<SelectedItemsResponse>.Success(content);
    }
}