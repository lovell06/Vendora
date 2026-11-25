namespace Vendora.Services.Identity.Application.Authentication;

public static class LogoutUser
{
    public sealed record Command(Guid UserId, Guid RefreshTokenId) : ICommand;

    public class Handler(
        ICurrentUser currentUserService,
        IAccessTokenProvider accessTokenProvider,
        IRefreshTokenProvider refreshTokenProvider) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            await refreshTokenProvider.RevokeAsync(command.UserId, command.RefreshTokenId, cancellationToken);

            await accessTokenProvider.RevokeAsync(currentUserService.GetAccessTokenId(), cancellationToken);

            return Result.Success();
        }
    }
}
