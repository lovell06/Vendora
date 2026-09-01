
using Vendora.BuildingBlocks.Cqrs;

namespace Vendora.Services.Identity.Application.Features.Authentication.VerifyEmail;

public record VerifyEmailCommand(Guid UserId, string Token) : ICommand;