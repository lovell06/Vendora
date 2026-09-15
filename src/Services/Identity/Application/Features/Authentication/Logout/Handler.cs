using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Identity.Application.Abstractions.Authentication;

namespace Vendora.Services.Identity.Application.Features.Authentication.Logout;

public class Handler(IRefreshTokenProvider refreshTokenProvider) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        await refreshTokenProvider.RevokeAsync(command.UserId, cancellationToken);

        return Result.Success();
    }
}