using Vendora.Services.Identity.Application.Features.Authentication.Logout;

namespace Vendora.Services.Identity.Api.Features.Auth.Logout;

public sealed class LogoutRequest
{
    public Guid UserId { get; init; }
    public required string RefreshToken { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            UserId = UserId,
            RefreshToken = RefreshToken
        };
    }
}