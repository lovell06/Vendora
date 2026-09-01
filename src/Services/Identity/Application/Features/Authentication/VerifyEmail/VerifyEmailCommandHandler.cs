using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Identity.Application.Abstractions.Email;
using Vendora.Services.Identity.Application.Abstractions.Persistence;
using Vendora.Services.Identity.Domain.Users;

namespace Vendora.Services.Identity.Application.Features.Authentication.VerifyEmail;

public class VerifyEmailCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IEmailVerificationTokenProvider tokenProvider,
    ILogger<VerifyEmailCommandHandler> logger,
    TimeProvider clock) : ICommandHandler<VerifyEmailCommand>
{
    public async Task<Result> Handle(VerifyEmailCommand command, CancellationToken cancellationToken)
    {
        var now = clock.GetUtcNow().UtcDateTime;

        if (command.UserId == Guid.Empty)
        {
            return Result.Failure(new Error
            {
                Code = "email_verification.user_id_empty",
                Message = "User id required.",
                Type = ErrorType.Validation
            });
        }

        if (string.IsNullOrWhiteSpace(command.Token))
        {
            return Result.Failure(new Error
            {
                Code = "email_verification.token_empty",
                Message = "Email verification token is required.",
                Type = ErrorType.Validation
            });
        }

        var user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
        {
            logger.LogWarning(
                "Email verification rejected because user: {userId} is not registered.",
                command.UserId);

            return Result.Failure(new Error
            {
                Code = "email_verification.failed",
                Message = "The email verification token is invalid or expired.",
                Type = ErrorType.Unauthorized
            });
        }

        var isValid = await tokenProvider.ValidateAsync(
            userId: user.Id, 
            token: command.Token, 
            cancellationToken: cancellationToken);

        if (!isValid)
        {
            logger.LogWarning(
                "Email verification rejected for user: {userId}.", 
                user.Id);
            
            return Result.Failure(new Error
            {
                Code = "email_verification.failed",
                Message = "The email verification token is invalid or expired.",
                Type = ErrorType.Unauthorized
            });
        }

        user.VerifyEmail(now);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await tokenProvider.RevokeAsync(user.Id, cancellationToken);
        
        return Result.Success();
    }
}