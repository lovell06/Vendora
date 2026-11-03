namespace Vendora.Services.Identity.Application.Users.CreateEmployeeUser;

public sealed class Handler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    TimeProvider clock,
    IPasswordHashProvider passwordHashProvider) : ICommandHandler<Command>
{
    public async Task<Result> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var utcNow = clock.GetUtcNow();

        if (await userRepository.ExistsByEmailAsync(cmd.Email, cancellationToken))
            return Result.Failure(new Error
            {
                Code = "email_existed",
                Message = "Email already exists.",
                Type = ErrorType.Conflict
            });

        var createEmployeeResult = User.CreateEmployee(
            email: cmd.Email,
            passwordHash: passwordHashProvider.Hash(cmd.Password),
            fullName: cmd.FullName,
            phoneNumber: cmd.PhoneNumber,
            createdAt: utcNow);

        if (createEmployeeResult.IsFailure)
            return Result.Failure(createEmployeeResult.Error);

        userRepository.Add(createEmployeeResult.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}