using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Authentication.Logout;

public sealed class Command : ICommand
{
    public Guid UserId { get; init; }
    public required string RefreshToken { get; init; }
}