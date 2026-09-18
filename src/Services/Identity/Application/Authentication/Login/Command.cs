using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Authentication.Login;

public sealed class Command : ICommand<Response>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}