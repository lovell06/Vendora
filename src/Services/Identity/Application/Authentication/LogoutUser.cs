namespace Vendora.Services.Identity.Application.Authentication;

public static class LogoutUser
{
    public sealed record Command(Guid UserId, string RefreshToken) : ICommand;

    public class Handler(IRefreshTokenProvider refreshTokenProvider) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            await refreshTokenProvider.RevokeAsync(command.UserId, cancellationToken);

            return Result.Success();
        }
    }
}
