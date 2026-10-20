using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Vendora.Services.Ordering.Infrastructure.Clients;

public class HttpInventoryServiceClient(HttpClient client) : IInventoryServiceClient
{
    public async Task<Result> ReserveAsync(IReadOnlyList<OrderItem> orderItems, CancellationToken ct)
    {
        using var message = new HttpRequestMessage(
            method: HttpMethod.Post,
            requestUri: "/api/inventory-items/reserve");
        
        message.Content = JsonContent.Create(new ReservedItemsDto
        {
            Items =
            [
                .. orderItems.Select(i => new ReservedItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                })
            ]
        });

        var response = await client.SendAsync(message, ct);

        if (response.IsSuccessStatusCode) return Result.Success();

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                return Result.Failure(new Error { Type = ErrorType.Unauthorized });
            case HttpStatusCode.Forbidden:
                return Result.Failure(new Error { Type = ErrorType.Forbidden });
            case HttpStatusCode.MethodNotAllowed:
                throw new InvalidOperationException(nameof(HttpStatusCode.MethodNotAllowed));
            default:
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(
                                        json: await response.Content.ReadAsStringAsync(ct))
                                    ?? throw new InvalidOperationException("Cannot convert JSON from HTTP Response.");

                return Result.Failure(Error.FromResponse(errorResponse));
            }
        }
    }
}

public struct ReservedItemDto
{
    public long ProductId { get; init; }
    public int Quantity { get; init; }
}

public sealed class ReservedItemsDto
{
    public required IReadOnlyList<ReservedItemDto> Items { get; init; }
}