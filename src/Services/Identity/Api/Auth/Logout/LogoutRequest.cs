using Vendora.Services.Identity.Application.Authentication;

namespace Vendora.Services.Identity.Api.Auth.Logout;

public sealed class LogoutRequest
{
    public const string Pattern = "/logout";
    
    public Guid UserId { get; init; }
    public required string RefreshToken { get; init; }

    public LogoutUser.Command ToCommand()
    {
        return new LogoutUser.Command(
            UserId,
            RefreshToken);
    }
}