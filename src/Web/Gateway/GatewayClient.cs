using System.Net;

namespace Vendora.Web.Gateway;

// The only place the Web talks HTTP to the backend. Every call goes through the API Gateway.
public sealed class GatewayClient(GatewayTransport transport, UserSession session)
{
    // ---- Catalog ----

    public Task<ApiResult<CategoryListDto>> ListCategoriesAsync(CancellationToken ct = default) =>
        transport.SendAsync<CategoryListDto>(HttpMethod.Get, "api/categories?page=1&size=100", ct: ct);

    public Task<ApiResult<ProductListDto>> ListProductsAsync(
        int? categoryId, string? search, int page, int size, CancellationToken ct = default)
    {
        var url = $"api/products?page={page}&size={size}";
        if (categoryId is not null) url += $"&categoryId={categoryId}";
        if (!string.IsNullOrWhiteSpace(search)) url += $"&search={Uri.EscapeDataString(search.Trim())}";
        return transport.SendAsync<ProductListDto>(HttpMethod.Get, url, ct: ct);
    }

    public async Task<ApiResult<ProductDetailDto>> GetProductAsync(long id, CancellationToken ct = default)
    {
        var result = await transport.SendAsync<ProductDetailResponse>(HttpMethod.Get, $"api/products/{id}", ct: ct);
        return result.Ok && result.Value is not null
            ? ApiResult<ProductDetailDto>.Success(result.Value.Product, result.Status)
            : ApiResult<ProductDetailDto>.Fail(result.Error ?? Vendora.Web.Localization.Text.Get("Unable to load this product."), result.Status);
    }

    public Task<ApiResult<VariantLookupResponse>> LookupVariantsAsync(
        IEnumerable<long> variantIds, CancellationToken ct = default) =>
        transport.SendAsync<VariantLookupResponse>(HttpMethod.Post, "api/products/batch",
            new { productVariantIds = variantIds.Distinct().ToList() }, ct: ct);

    // ---- Identity ----

    public Task<ApiResult<LoginResponse>> LoginAsync(string email, string password, CancellationToken ct = default) =>
        transport.SendAsync<LoginResponse>(HttpMethod.Post, "api/auth/login", new { email, password }, ct: ct);

    public Task<ApiResult<NoContent>> RegisterAsync(
        string fullName, string email, string password, string phoneNumber, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Post, "api/auth/register",
            new { email, password, fullName, phoneNumber }, ct: ct);

    public Task<ApiResult<NoContent>> VerifyEmailAsync(Guid userId, string token, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Post, "api/auth/verify-email", new { userId, token }, ct: ct);

    public async Task<ApiResult<NoContent>> LogoutAsync(CancellationToken ct = default)
    {
        await session.EnsureLoadedAsync();
        if (session.Current is not { } current)
            return ApiResult<NoContent>.Success(default, HttpStatusCode.NoContent);
        return await transport.SendAsync<NoContent>(HttpMethod.Post, "api/auth/logout",
            new { refreshToken = current.RefreshTokenId }, auth: true, ct: ct);
    }

    // ---- Cart (requires a signed-in user) ----

    public Task<ApiResult<CartResponse>> GetCartAsync(CancellationToken ct = default) =>
        transport.SendAsync<CartResponse>(HttpMethod.Get, "api/carts/list", auth: true, ct: ct);

    public Task<ApiResult<NoContent>> AddToCartAsync(long variantId, int quantity, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Post, "api/carts/add-item",
            new { productVariantId = variantId, quantity }, auth: true, ct: ct);

    public Task<ApiResult<NoContent>> SetCartQuantityAsync(long variantId, int quantity, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Patch, $"api/carts/update/{variantId}", new { quantity }, auth: true, ct: ct);

    public Task<ApiResult<NoContent>> RemoveFromCartAsync(long variantId, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Delete, $"api/carts/remove-item/{variantId}", auth: true, ct: ct);

    // ---- Ordering ----

    // Places an order for the given cart items (the backend reads them from the signed-in user's cart).
    public Task<ApiResult<NoContent>> PlaceOrderAsync(IEnumerable<long> variantIds, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Post, "api/orders/place",
            new { productVariantIds = variantIds.Distinct().ToList() }, auth: true, ct: ct);
}
