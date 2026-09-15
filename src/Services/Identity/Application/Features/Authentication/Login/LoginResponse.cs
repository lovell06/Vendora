namespace Vendora.Services.Identity.Application.Features.Authentication.Login;

public sealed class LoginResponse
{
    public Guid UserId { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}