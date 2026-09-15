using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Features.Authentication.Register;

public sealed class RegisterCommand : ICommand
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }
}