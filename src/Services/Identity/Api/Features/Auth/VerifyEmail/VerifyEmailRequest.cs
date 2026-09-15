using Vendora.Services.Identity.Application.Features.Authentication.VerifyEmail;

namespace Vendora.Services.Identity.Api.Features.Auth.VerifyEmail;

public sealed class VerifyEmailRequest
{
    public Guid UserId { get; init; }
    public required string Token { get; init; }
    
    public VerifyEmailCommand ToCommand()
    {
        return new VerifyEmailCommand
        {
            UserId = UserId,
            Token = Token
        };
    }
}