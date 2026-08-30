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