using Vendora.Services.Identity.Application.Features.Authentication.VerifyEmail;

namespace Vendora.Services.Identity.Api.Features.Auth.VerifyEmail;

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