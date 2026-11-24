using Vendora.Services.Identity.Application.Users;

namespace Vendora.Services.Identity.Api.Users.CreateEmployeeUser;

public sealed class CreateEmployeeUserRequest
{
    public const string Pattern = "/employees/create";

    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }

    public CreateEmployee.Command ToCommand()
    {
        return new CreateEmployee.Command(
            Email,
            Password,
            FullName,
            PhoneNumber);
    }
}