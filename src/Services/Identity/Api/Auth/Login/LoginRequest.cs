using Vendora.Services.Identity.Application.Authentication;

namespace Vendora.Services.Identity.Api.Auth.Login;

public sealed class LoginRequest
{
    public const string Pattern = "/login";
    
    public required string Email { get; init; }
    public required string Password { get; init; }

    public LoginUser.Command ToCommand()
    {
        return new LoginUser.Command(
            Email,
            Password);
    }
}