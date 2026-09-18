using Vendora.Services.Identity.Application.Authentication.Register;

namespace Vendora.Services.Identity.Api.Auth.Register;

public sealed class RegisterRequest
{
    public const string Pattern = "/register";
    
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Email = Email,
            FullName = FullName,
            Password = Password,
            PhoneNumber = PhoneNumber
        };
    }
}