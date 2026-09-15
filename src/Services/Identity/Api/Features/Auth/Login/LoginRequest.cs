using Vendora.Services.Identity.Application.Features.Authentication.Login;

namespace Vendora.Services.Identity.Api.Features.Auth.Login;

public sealed class LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    
    public LoginCommand ToCommand()
    {
        return new LoginCommand
        {
            Email = Email,
            Password = Password
        };
    }
}