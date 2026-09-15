
using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Features.Authentication.VerifyEmail;

public sealed class VerifyEmailCommand : ICommand
{
    public Guid UserId { get; init; }
    public required string Token { get; init; }
}