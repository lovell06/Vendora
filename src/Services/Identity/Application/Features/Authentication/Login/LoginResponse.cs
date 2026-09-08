namespace Vendora.Services.Identity.Application.Features.Authentication.Login;

public record LoginResponse(
    Guid UserId,
    string AccessToken,
    string RefreshToken);