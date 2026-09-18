namespace Vendora.Services.Identity.Application.Authentication.Login;

public sealed class Response
{
    public Guid UserId { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}