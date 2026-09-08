using Vendora.Services.Identity.Application.Features.Authentication.Logout;

namespace Vendora.Services.Identity.Api.Features.Auth.Logout;

public record LogoutRequest(Guid UserId, string RefreshToken)
{
    public LogoutCommand ToCommand()
    {
        return new LogoutCommand(UserId, RefreshToken);
    }
}