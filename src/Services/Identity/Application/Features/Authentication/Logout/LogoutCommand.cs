using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Features.Authentication.Logout;

public sealed class LogoutCommand : ICommand
{
    public Guid UserId { get; init; }
    public required string RefreshToken { get; init; }
}