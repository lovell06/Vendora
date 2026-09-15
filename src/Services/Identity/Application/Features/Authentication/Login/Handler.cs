using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Identity.Application.Abstractions.Authentication;
using Vendora.Services.Identity.Domain.Users;

namespace Vendora.Services.Identity.Application.Features.Authentication.Login;

public class Handler(
    IUserRepository userRepository,
    IPasswordHashProvider passwordHashProvider,
    IAccessTokenProvider accessTokenProvider,
    IRefreshTokenProvider refreshTokenProvider,
    ILogger<Handler> logger) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
    {
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

        return Result<Response>.Success(new Response
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }
}