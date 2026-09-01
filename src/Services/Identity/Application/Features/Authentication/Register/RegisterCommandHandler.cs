using Microsoft.Extensions.Logging;
using Vendora.BuildingBlocks.Cqrs;
using Vendora.BuildingBlocks.Results;
using Vendora.Services.Identity.Application.Abstractions.Authentication;
using Vendora.Services.Identity.Application.Abstractions.Email;
using Vendora.Services.Identity.Application.Abstractions.Persistence;
using Vendora.Services.Identity.Domain.Users;

namespace Vendora.Services.Identity.Application.Features.Authentication.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHashProvider passwordHashProvider,
    IEmailSender emailSender,
    IEmailVerificationTokenProvider emailVerificationTokenProvider,
    ILogger<RegisterCommandHandler> logger,
    TimeProvider clock): ICommandHandler<RegisterCommand>
{
    public async Task<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow().UtcDateTime;

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            logger.LogWarning("Registration rejected because email empty.");
            
            return Result.Failure(new Error
            {
                Code = "email_empty",
                Message = "Email required.",
                Type = ErrorType.Validation
            });
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            logger.LogWarning("Registration rejected because password empty.");
            
            return Result.Failure(new Error
            {
                Code = "password_empty",
                Message = "Password required.",
                Type = ErrorType.Validation
            });
        }

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            logger.LogWarning("Registration rejected because full name empty.");
            
            return Result.Failure(new Error
            {
                Code = "full_name_empty",
                Message = "Full name required.",
                Type = ErrorType.Validation
            });
        }

        if (string.IsNullOrWhiteSpace(command.PhoneNumber))
        {
            logger.LogWarning("Registration rejected because phone number empty.");
            
            return Result.Failure(new Error
            {
                Code = "phone_number_empty",
                Message = "Phone number required.",
                Type = ErrorType.Validation
            });
        }

        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
        {
            logger.LogWarning("Registration rejected because email already exists.");
            
            return Result.Failure(new Error
            {
                Code = "email_existed",
                Message = "Email already exists.",
                Type = ErrorType.Conflict
            });
        }

        var passwordHash = passwordHashProvider.Hash(command.Password);

        var user = User.CreateCustomer(
            email: command.Email,
            passwordHash: passwordHash,
            fullName: command.FullName,
            phoneNumber: command.PhoneNumber,
            createdAt: utcNow);

        userRepository.Add(user);

        var affectedRows = await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("{affectedRows} rows affected.", affectedRows);

        var verificationToken = await emailVerificationTokenProvider.IssueAsync(
            user.Id,
            cancellationToken);

        await emailSender.SendAsync(
            recepient: user.Email,
            subject: "Verify email",
            body: $"https://default.com?userId={user.Id}&token={verificationToken}",
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}