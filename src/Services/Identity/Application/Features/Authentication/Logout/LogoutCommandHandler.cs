using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Identity.Application.Abstractions.Authentication;

namespace Vendora.Services.Identity.Application.Features.Authentication.Logout;

public class LogoutCommandHandler(IRefreshTokenProvider refreshTokenProvider) : ICommandHandler<LogoutCommand>
{
    public async Task<Result> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        await refreshTokenProvider.RevokeAsync(command.UserId, cancellationToken);

        return Result.Success();
    }
}