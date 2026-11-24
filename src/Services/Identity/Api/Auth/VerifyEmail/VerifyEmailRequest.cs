using Vendora.Services.Identity.Application.Authentication;

namespace Vendora.Services.Identity.Api.Auth.VerifyEmail;

public sealed class VerifyEmailRequest
{
    public const string Pattern = "/verify-email";
    
    public Guid UserId { get; init; }
    public required string Token { get; init; }

    public VerifyUserEmail.Command ToCommand()
    {
        return new VerifyUserEmail.Command(
            UserId,
            Token);
    }
}