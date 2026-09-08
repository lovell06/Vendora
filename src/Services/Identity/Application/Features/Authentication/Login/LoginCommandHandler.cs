using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Identity.Application.Abstractions.Authentication;
using Vendora.Services.Identity.Domain.Users;

namespace Vendora.Services.Identity.Application.Features.Authentication.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHashProvider passwordHashProvider,
    IAccessTokenProvider accessTokenProvider,
    IRefreshTokenProvider refreshTokenProvider,
    ILogger<LoginCommandHandler> logger) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null)
        {
            logger.LogWarning("Login rejected because user is not found.");

            return Result<LoginResponse>.Failure(new Error
            {
                Code = "login.unauthorized",
                Message = "Email or password invalid.",
                Type = ErrorType.Unauthorized
            });
        }

        if (!passwordHashProvider.Verify(user.PasswordHash, command.Password))
        {
            logger.LogWarning("Login rejected because password invalid.");

            return Result<LoginResponse>.Failure(new Error
            {
                Code = "login.unauthorized",
                Message = "Email or password invalid.",
                Type = ErrorType.Unauthorized
            });
        }

        var accessToken = accessTokenProvider.Issue(user);
        var refreshToken = await refreshTokenProvider.IssueAsync(user.Id, cancellationToken);

        return Result<LoginResponse>.Success(new LoginResponse(
            UserId: user.Id,
            AccessToken: accessToken,
            RefreshToken: refreshToken));
    }
}