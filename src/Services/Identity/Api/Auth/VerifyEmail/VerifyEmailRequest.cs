using Vendora.Services.Identity.Application.Authentication.VerifyEmail;

namespace Vendora.Services.Identity.Api.Auth.VerifyEmail;

public sealed class VerifyEmailRequest
{
    public Guid UserId { get; init; }
    public required string Token { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            UserId = UserId,
            Token = Token
        };
    }
}