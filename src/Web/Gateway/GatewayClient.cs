using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Vendora.Web.Gateway;

// The only place the Web talks HTTP to the backend. Every call goes through the API Gateway.
public sealed class GatewayClient(HttpClient http, UserSession session)
{
    // ---- Catalog ----

    public Task<ApiResult<CategoryListDto>> ListCategoriesAsync(CancellationToken ct = default) =>
        SendAsync<CategoryListDto>(HttpMethod.Get, "api/categories?page=1&size=100", ct: ct);

    public Task<ApiResult<ProductListDto>> ListProductsAsync(
        int? categoryId, string? search, int page, int size, CancellationToken ct = default)
    {
        var url = $"api/products?page={page}&size={size}";
        if (categoryId is not null) url += $"&categoryId={categoryId}";
        if (!string.IsNullOrWhiteSpace(search)) url += $"&search={Uri.EscapeDataString(search.Trim())}";
        return SendAsync<ProductListDto>(HttpMethod.Get, url, ct: ct);
    }

    public async Task<ApiResult<ProductDetailDto>> GetProductAsync(long id, CancellationToken ct = default)
    {
        var result = await SendAsync<ProductDetailResponse>(HttpMethod.Get, $"api/products/{id}", ct: ct);
        return result.Ok && result.Value is not null
            ? ApiResult<ProductDetailDto>.Success(result.Value.Product, result.Status)
            : ApiResult<ProductDetailDto>.Fail(result.Error ?? "Unable to load this product.", result.Status);
    }

    public Task<ApiResult<VariantLookupResponse>> LookupVariantsAsync(
        IEnumerable<long> variantIds, CancellationToken ct = default) =>
        SendAsync<VariantLookupResponse>(HttpMethod.Post, "api/products/batch",
            new { productVariantIds = variantIds.Distinct().ToList() }, ct: ct);

    // ---- Identity ----

    public Task<ApiResult<LoginResponse>> LoginAsync(string email, string password, CancellationToken ct = default) =>
        SendAsync<LoginResponse>(HttpMethod.Post, "api/auth/login", new { email, password }, ct: ct);

    public Task<ApiResult<NoContent>> RegisterAsync(
        string fullName, string email, string password, string phoneNumber, CancellationToken ct = default) =>
        SendAsync<NoContent>(HttpMethod.Post, "api/auth/register",
            new { email, password, fullName, phoneNumber }, ct: ct);

    public Task<ApiResult<NoContent>> VerifyEmailAsync(Guid userId, string token, CancellationToken ct = default) =>
        SendAsync<NoContent>(HttpMethod.Post, "api/auth/verify-email", new { userId, token }, ct: ct);

    public async Task<ApiResult<NoContent>> LogoutAsync(CancellationToken ct = default)
    {
        await session.EnsureLoadedAsync();
        if (session.Current is not { } current)
            return ApiResult<NoContent>.Success(default, HttpStatusCode.NoContent);
        return await SendAsync<NoContent>(HttpMethod.Post, "api/auth/logout",
            new { userId = current.UserId, refreshToken = current.RefreshToken }, auth: true, ct: ct);
    }

    // ---- Cart (requires a signed-in user) ----

    public Task<ApiResult<CartResponse>> GetCartAsync(CancellationToken ct = default) =>
        SendAsync<CartResponse>(HttpMethod.Get, "api/carts/list", auth: true, ct: ct);

    public Task<ApiResult<NoContent>> AddToCartAsync(long variantId, int quantity, CancellationToken ct = default) =>
        SendAsync<NoContent>(HttpMethod.Post, "api/carts/add-item",
            new { productVariantId = variantId, quantity }, auth: true, ct: ct);

    public Task<ApiResult<NoContent>> SetCartQuantityAsync(long variantId, int quantity, CancellationToken ct = default) =>
        SendAsync<NoContent>(HttpMethod.Patch, $"api/carts/update/{variantId}", new { quantity }, auth: true, ct: ct);

    public Task<ApiResult<NoContent>> RemoveFromCartAsync(long variantId, CancellationToken ct = default) =>
        SendAsync<NoContent>(HttpMethod.Delete, $"api/carts/remove-item/{variantId}", auth: true, ct: ct);

    // ---- Ordering ----

    // Places an order for the given cart items (the backend reads them from the signed-in user's cart).
    public Task<ApiResult<NoContent>> PlaceOrderAsync(IEnumerable<long> variantIds, CancellationToken ct = default) =>
        SendAsync<NoContent>(HttpMethod.Post, "api/orders/place",
            new { productVariantIds = variantIds.Distinct().ToList() }, auth: true, ct: ct);

    // ---- Plumbing ----

    private async Task<ApiResult<T>> SendAsync<T>(
        HttpMethod method, string url, object? body = null, bool auth = false, CancellationToken ct = default)
    {
        try
        {
            using var request = new HttpRequestMessage(method, url);
            if (body is not null) request.Content = JsonContent.Create(body);
            if (auth)
            {
                await session.EnsureLoadedAsync();
                if (!session.IsAuthenticated)
                    return ApiResult<T>.Fail("Please sign in to continue.", HttpStatusCode.Unauthorized);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.Current!.AccessToken);
            }

            using var response = await http.SendAsync(request, ct);

            if (response.IsSuccessStatusCode)
            {
                if (typeof(T) == typeof(NoContent))
                    return ApiResult<T>.Success(default!, response.StatusCode);

                var value = await response.Content.ReadFromJsonAsync<T>(ct);
                return value is null
                    ? ApiResult<T>.Fail("The server returned an empty response.", response.StatusCode)
                    : ApiResult<T>.Success(value, response.StatusCode);
            }

            if (auth && response.StatusCode == HttpStatusCode.Unauthorized)
                await session.SignOutAsync(); // The backend has no refresh endpoint; an expired token ends the session.

            return ApiResult<T>.Fail(await ReadErrorAsync(response, ct), response.StatusCode);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            if (ct.IsCancellationRequested) throw;
            return ApiResult<T>.Fail("Unable to connect. Check your connection and try again.", HttpStatusCode.ServiceUnavailable);
        }
    }

    // Backend errors are { code, message, type }.
    private static async Task<string> ReadErrorAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            var text = await response.Content.ReadAsStringAsync(ct);
            if (!string.IsNullOrWhiteSpace(text))
            {
                using var doc = JsonDocument.Parse(text);
                if (doc.RootElement.ValueKind == JsonValueKind.Object
                    && doc.RootElement.TryGetProperty("message", out var message)
                    && message.ValueKind == JsonValueKind.String
                    && !string.IsNullOrWhiteSpace(message.GetString()))
                    return message.GetString()!;
            }
        }
        catch (JsonException) { }

        return response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => "Please sign in to continue.",
            HttpStatusCode.Forbidden => "You do not have permission to do this.",
            HttpStatusCode.NotFound => "We could not find what you asked for.",
            HttpStatusCode.Conflict => "That conflicts with the current state. Please try again.",
            _ => "Unable to complete your request. Please try again later."
        };
    }
}
