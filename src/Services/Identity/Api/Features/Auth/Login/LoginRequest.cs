using Vendora.Services.Identity.Application.Features.Authentication.Login;

namespace Vendora.Services.Identity.Api.Features.Auth.Login;

public record LoginRequest(string Email, string Password)
{
    public LoginCommand ToCommand()
    {
        return new LoginCommand(Email, Password);
    }
}