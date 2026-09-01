using Vendora.Services.Identity.Application.Features.Authentication.VerifyEmail;

namespace Vendora.Services.Identity.Api.Features.Auth.VerifyEmail;

public record VerifyEmailRequest(Guid UserId, string Token)
{
    public VerifyEmailCommand ToCommand()
    {
        return new VerifyEmailCommand(UserId, Token);
    }
}