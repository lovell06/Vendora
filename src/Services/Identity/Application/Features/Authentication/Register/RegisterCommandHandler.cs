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
            var err = new Error
            {
                Code = "email_empty",
                Message = "Email required.",
                Type = ErrorType.Validation
            };
            logger.LogWarning("Registration rejected because email empty. {utcNow}", utcNow);
            return Result.Failure(err);
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            var err = new Error
            {
                Code = "password_empty",
                Message = "Password required.",
                Type = ErrorType.Validation
            };
            logger.LogWarning("Registration rejected because password empty. {utcNow}", utcNow);
            return Result.Failure(err);
        }

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            var err = new Error
            {
                Code = "full_name_empty",
                Message = "Full name required.",
                Type = ErrorType.Validation
            };
            logger.LogWarning("Registration rejected because full name empty. {utcNow}", utcNow);
            return Result.Failure(err);
        }

        if (string.IsNullOrWhiteSpace(command.PhoneNumber))
        {
            var err = new Error
            {
                Code = "phone_number_empty",
                Message = "Phone number required.",
                Type = ErrorType.Validation
            };
            logger.LogWarning("Registration rejected because phone number empty. {utcNow}", utcNow);
            return Result.Failure(err);
        }

        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
        {
            var err = new Error
            {
                Code = "email_existed",
                Message = "Email already exists.",
                Type = ErrorType.Conflict
            };
            logger.LogWarning("Registration rejected because email already exists. {utcNow}", utcNow);
            return Result.Failure(err);
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

        logger.LogInformation("{affectedRows} rows affected. {utcNow}", affectedRows, utcNow);

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