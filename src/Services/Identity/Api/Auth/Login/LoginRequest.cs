using Vendora.Services.Identity.Application.Authentication.Login;

namespace Vendora.Services.Identity.Api.Auth.Login;

public sealed class LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Email = Email,
            Password = Password
        };
    }
}