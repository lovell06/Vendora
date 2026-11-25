using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Vendora.Web.Gateway;

// HTTP plumbing shared by GatewayClient (storefront) and AdminGatewayClient (back office).
public sealed class GatewayTransport(HttpClient http, UserSession session)
{
    public Task<ApiResult<T>> SendAsync<T>(
        HttpMethod method, string url, object? body = null, bool auth = false, CancellationToken ct = default) =>
        SendCoreAsync<T>(method, url, body is null ? null : JsonContent.Create(body), auth, ct);

    public Task<ApiResult<T>> SendFormAsync<T>(
        HttpMethod method, string url, HttpContent form, bool auth = true, CancellationToken ct = default) =>
        SendCoreAsync<T>(method, url, form, auth, ct);

    private async Task<ApiResult<T>> SendCoreAsync<T>(
        HttpMethod method, string url, HttpContent? content, bool auth, CancellationToken ct)
    {
        try
        {
            using var request = new HttpRequestMessage(method, url) { Content = content };
            if (auth)
            {
                await session.EnsureLoadedAsync();
                if (!session.IsAuthenticated)
                    return ApiResult<T>.Fail(Vendora.Web.Localization.Text.Get("Please sign in to continue."), HttpStatusCode.Unauthorized);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.Current!.AccessToken);
            }

            using var response = await http.SendAsync(request, ct);

            if (response.IsSuccessStatusCode)
            {
                if (typeof(T) == typeof(NoContent))
                    return ApiResult<T>.Success(default!, response.StatusCode);

                var value = await response.Content.ReadFromJsonAsync<T>(ct);
                return value is null
                    ? ApiResult<T>.Fail(Vendora.Web.Localization.Text.Get("The server returned an empty response."), response.StatusCode)
                    : ApiResult<T>.Success(value, response.StatusCode);
            }

            if (auth && response.StatusCode == HttpStatusCode.Unauthorized)
                await session.SignOutAsync(); // The backend has no refresh endpoint; an expired token ends the session.

            var (message, code) = await ReadErrorAsync(response, ct);
            return ApiResult<T>.Fail(message, response.StatusCode, code);
        }
        catch (JsonException)
        {
            return ApiResult<T>.Fail(Vendora.Web.Localization.Text.Get("Unexpected response from the server."), HttpStatusCode.BadGateway);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            if (ct.IsCancellationRequested) throw;
            return ApiResult<T>.Fail(Vendora.Web.Localization.Text.Get("Unable to connect. Check your connection and try again."), HttpStatusCode.ServiceUnavailable);
        }
    }

    // Backend errors are { code, message, type }.
    private static async Task<(string Message, string? Code)> ReadErrorAsync(HttpResponseMessage response, CancellationToken ct)
    {
        string? code = null;
        try
        {
            var text = await response.Content.ReadAsStringAsync(ct);
            if (!string.IsNullOrWhiteSpace(text))
            {
                using var doc = JsonDocument.Parse(text);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("code", out var c) && c.ValueKind == JsonValueKind.String)
                        code = c.GetString();
                    if (root.TryGetProperty("message", out var message)
                        && message.ValueKind == JsonValueKind.String
                        && !string.IsNullOrWhiteSpace(message.GetString()))
                        return (message.GetString()!, code);
                }
            }
        }
        catch (JsonException) { }

        return (Vendora.Web.Localization.Text.Get(response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => "Please sign in to continue.",
            HttpStatusCode.Forbidden => "You do not have permission to do this.",
            HttpStatusCode.NotFound => "We could not find what you asked for.",
            HttpStatusCode.Conflict => "That conflicts with the current state. Please try again.",
            HttpStatusCode.RequestEntityTooLarge => "The upload is too large.",
            _ => "Unable to complete your request. Please try again later."
        }), code);
    }
}
