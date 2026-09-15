using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Features.Authentication.Login;

public sealed class LoginCommand : ICommand<LoginResponse>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}