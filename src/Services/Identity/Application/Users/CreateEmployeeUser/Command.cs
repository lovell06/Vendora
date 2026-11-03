namespace Vendora.Services.Identity.Application.Users.CreateEmployeeUser;

public sealed class Command : ICommand
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }
}