namespace Vendora.Services.Identity.Api.Auth.Logout;

public sealed class LogoutRequest
{
    public const string Pattern = "/logout";
    
    public required Guid RefreshToken { get; init; }
}