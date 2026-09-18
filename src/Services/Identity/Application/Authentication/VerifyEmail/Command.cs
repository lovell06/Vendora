
using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Authentication.VerifyEmail;

public sealed class Command : ICommand
{
    public Guid UserId { get; init; }
    public required string Token { get; init; }
}