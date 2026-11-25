namespace Vendora.Services.Identity.Application.Authentication;

public static class LoginUser
{
    public sealed record Command(string Email, string Password) : ICommand<Response>;

    public sealed record Response(Guid UserId, string AccessToken, RefreshToken RefreshToken);

    public class Handler(
        IUserRepository userRepository,
        IPasswordHashProvider passwordHashProvider,
        IAccessTokenProvider accessTokenProvider,
        IRefreshTokenProvider refreshTokenProvider,
        ILogger<Handler> logger) : ICommandHandler<Command, Response>
    {
        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            if (!await userRepository.IsEmailVerification(command.Email, cancellationToken))
                return Result<Response>.Failure(Error.Unauthorized());
            
            var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("Login rejected because user is not found.");

                return Result<Response>.Failure(new Error
                {
                    Code = "login.unauthorized",
                    Message = "Email or password invalid.",
                    Type = ErrorType.Unauthorized
                });
            }

            if (!passwordHashProvider.Verify(user.PasswordHash, command.Password))
            {
                logger.LogWarning("Login rejected because password invalid.");

                return Result<Response>.Failure(new Error
                {
                    Code = "login.unauthorized",
                    Message = "Email or password invalid.",
                    Type = ErrorType.Unauthorized
                });
            }

            var accessToken = accessTokenProvider.Issue(user);
            var refreshToken = await refreshTokenProvider.IssueAsync(user.Id, cancellationToken);

            return Result<Response>.Success(new Response(
                user.Id,
                accessToken,
                refreshToken));
        }
    }
}
