using System.Globalization;

namespace Vendora.Web.Gateway.Admin;

// Back-office calls to the Catalog API through the Gateway. Reads of public data are anonymous; writes send the bearer token.
public sealed class AdminGatewayClient(GatewayTransport transport)
{
    public const string Currency = "VND";

    // ---- Categories (A1–A6) ----

    public Task<ApiResult<CategoryListDto>> ListCategoriesAsync(int page = 1, int size = 100, CancellationToken ct = default) =>
        transport.SendAsync<CategoryListDto>(HttpMethod.Get, $"api/categories?page={page}&size={size}", ct: ct);

    public Task<ApiResult<CategoryDto>> GetCategoryAsync(int id, CancellationToken ct = default) =>
        transport.SendAsync<CategoryDto>(HttpMethod.Get, $"api/categories/{id}", ct: ct);

    public Task<ApiResult<NoContent>> CreateCategoryAsync(string name, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Post, "api/categories/create", new { name }, auth: true, ct: ct);

    public Task<ApiResult<NoContent>> UpdateCategoryAsync(int id, string name, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Put, "api/categories/update", new { id, name }, auth: true, ct: ct);

    public Task<ApiResult<NoContent>> DeleteCategoryAsync(int id, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Delete, $"api/categories/delete/{id}", auth: true, ct: ct);

    public Task<ApiResult<NoContent>> RestoreCategoryAsync(int id, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Patch, $"api/categories/restore/{id}", auth: true, ct: ct);

    // ---- Products (A7–A14) ----

    public Task<ApiResult<ProductListDto>> ListProductsAsync(
        int page, int size, int? categoryId = null, string? search = null, CancellationToken ct = default)
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
            : ApiResult<ProductDetailDto>.Fail(result.Error ?? "Unable to load this product.", result.Status, result.Code);
    }

    public async Task<ApiResult<long>> CreateProductAsync(CreateProductInput input, CancellationToken ct = default)
    {
        var inv = CultureInfo.InvariantCulture;
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(input.Name), "Name");
        form.Add(new StringContent(input.Description ?? ""), "Description");
        form.Add(new StringContent(input.Brand), "Brand");
        form.Add(new StringContent(input.CategoryId.ToString(inv)), "CategoryId");
        form.Add(new StringContent(Currency), "Currency");
        form.Add(new StringContent(input.IsVisible ? "true" : "false"), "IsVisible");

        for (var i = 0; i < input.Variants.Count; i++)
        {
            var v = input.Variants[i];
            form.Add(new StringContent(v.Sku), $"ProductVariants[{i}].Sku");
            form.Add(new StringContent(v.Name), $"ProductVariants[{i}].Name");
            form.Add(new StringContent(v.Price.ToString(inv)), $"ProductVariants[{i}].Price");
            for (var j = 0; j < v.ImageIndexes.Count; j++)
                form.Add(new StringContent(v.ImageIndexes[j].ToString(inv)), $"ProductVariants[{i}].ImageIndexes[{j}]");
        }

        var streams = new List<Stream>();
        try
        {
            foreach (var image in input.Images)
            {
                var stream = image.OpenRead();
                streams.Add(stream);
                var part = new StreamContent(stream);
                part.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
                form.Add(part, "Images", image.FileName);
            }

            var result = await transport.SendFormAsync<CreatedProductDto>(HttpMethod.Post, "api/products/create", form, ct: ct);
            return result.Ok && result.Value is not null
                ? ApiResult<long>.Success(result.Value.Id, result.Status)
                : ApiResult<long>.Fail(result.Error ?? "Unable to create the product.", result.Status, result.Code);
        }
        finally
        {
            foreach (var s in streams) await s.DisposeAsync();
        }
    }

    public Task<ApiResult<NoContent>> UpdateProductAsync(
        long id, string name, string? description, string brand, int categoryId, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Put, "api/products/update",
            new { id, name, description, brand, categoryId }, auth: true, ct: ct);

    public Task<ApiResult<NoContent>> DiscontinueProductAsync(long id, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Patch, $"api/products/discontinue/{id}", auth: true, ct: ct);

    public Task<ApiResult<NoContent>> ReactivateProductAsync(long id, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Patch, $"api/products/reactivate/{id}", auth: true, ct: ct);

    public Task<ApiResult<NoContent>> DeleteProductAsync(long id, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Delete, $"api/products/delete/{id}", auth: true, ct: ct);

    public Task<ApiResult<NoContent>> RestoreProductAsync(long id, CancellationToken ct = default) =>
        transport.SendAsync<NoContent>(HttpMethod.Patch, $"api/products/restore/{id}", auth: true, ct: ct);
}
