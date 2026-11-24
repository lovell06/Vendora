using Vendora.Services.Identity.Application.Authentication;

namespace Vendora.Services.Identity.Api.Auth.Register;

public sealed class RegisterRequest
{
    public const string Pattern = "/register";
    
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }

    public RegisterUser.Command ToCommand()
    {
        return new RegisterUser.Command(
            Email,
            Password,
            FullName,
            PhoneNumber);
    }
}