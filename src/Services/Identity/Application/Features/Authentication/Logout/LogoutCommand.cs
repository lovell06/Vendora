using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Features.Authentication.Logout;

public record LogoutCommand(Guid UserId, string RefreshToken) : ICommand;