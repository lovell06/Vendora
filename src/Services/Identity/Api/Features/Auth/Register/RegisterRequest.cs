using Vendora.Services.Identity.Application.Features.Authentication.Register;

namespace Vendora.Services.Identity.Api.Features.Auth.Register;

public sealed class RegisterRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }

    public RegisterCommand ToCommand()
    {
        return new RegisterCommand
        {
            Email = Email,
            FullName = FullName,
            Password = Password,
            PhoneNumber = PhoneNumber
        };
    }
}