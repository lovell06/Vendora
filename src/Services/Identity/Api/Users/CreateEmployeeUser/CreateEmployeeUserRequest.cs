using Vendora.Services.Identity.Application.Users.CreateEmployeeUser;

namespace Vendora.Services.Identity.Api.Users.CreateEmployeeUser;

public sealed class CreateEmployeeUserRequest
{
    public const string Pattern = "/employees/create";

    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }

    public Command ToCommand()
    {
        return new Command
        {
            Email = Email,
            Password = Password,
            FullName = FullName,
            PhoneNumber = PhoneNumber
        };
    }
}