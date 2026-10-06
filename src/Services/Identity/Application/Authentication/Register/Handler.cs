namespace Vendora.Services.Identity.Application.Authentication.Register;

public class Handler(
    IUserRepository userRepository,
    IOutboxRepository outboxRepository,
    IUnitOfWork unitOfWork,
    IPasswordHashProvider passwordHashProvider,
    ILogger<Handler> logger,
    TimeProvider clock): ICommandHandler<Command>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

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

        var createdUserResult = User.CreateCustomer(
            email: command.Email,
            passwordHash: passwordHash,
            fullName: command.FullName,
            phoneNumber: command.PhoneNumber,
            createdAt: utcNow);

        if (createdUserResult.IsFailure)
        {
            return Result.Failure(createdUserResult.Error);
        }

        var user = createdUserResult.Value;

        userRepository.Add(user);

        outboxRepository.Add(new UserRegisteredEvent(
            Id: Guid.CreateVersion7(),
            OccurredAt: utcNow,
            UserId: user.Id,
            UserEmail: user.Email), utcNow);

        var affectedRows = await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("{affectedRows} rows affected.", affectedRows);

        return Result.Success();
    }
}
