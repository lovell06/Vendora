using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Features.Authentication.Login;

public record LoginCommand(
    string Email,
    string Password) : ICommand<LoginResponse>;