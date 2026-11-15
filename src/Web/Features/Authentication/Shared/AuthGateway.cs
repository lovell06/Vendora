using Microsoft.Extensions.Options;

namespace Vendora.Web.Features.Authentication.Shared;

public sealed class AuthGatewayOptions
{
    public string BaseUrl { get; set; } = "";
    public string LoginPath { get; set; } = "";
    public string RegisterPath { get; set; } = "";
}

public sealed record AuthResult(bool Success, string Message);

// All authentication requests go through the configured API Gateway, never directly to Services.
public sealed class AuthGateway(HttpClient http, IOptions<AuthGatewayOptions> options)
{
    public async Task<AuthResult> SubmitAsync<T>(T request, bool register)
    {
        var settings = options.Value;
        var path = register ? settings.RegisterPath : settings.LoginPath;
        if (!Uri.TryCreate(settings.BaseUrl, UriKind.Absolute, out var root) || string.IsNullOrWhiteSpace(path))
            return new(false, "Sign in and registration are currently unavailable. Please try again later.");
        try
        {
            using var response = await http.PostAsJsonAsync(new Uri(root, path), request);
            if (!response.IsSuccessStatusCode)
                return new(false, response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    ? "Incorrect email or password. Please try again."
                    : response.StatusCode == System.Net.HttpStatusCode.Conflict
                        ? "This email is already in use. Sign in or use another email."
                        : "Unable to complete your request. Please try again later.");
            return new(true, register ? "Account created successfully." : "Credentials accepted. Unable to start a session yet.");
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return new(false, "Unable to connect. Check your connection and try again.");
        }
    }
}
