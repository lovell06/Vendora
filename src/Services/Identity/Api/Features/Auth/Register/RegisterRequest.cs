using Vendora.Services.Identity.Application.Features.Authentication.Register;

namespace Vendora.Services.Identity.Api.Features.Auth.Register;

public record RegisterRequest(
    string Email,
    string Password,
    string FullName,
    string PhoneNumber)
{
    public RegisterCommand ToCommand()
    {
        return new RegisterCommand(Email, Password, FullName, PhoneNumber);
    }
}