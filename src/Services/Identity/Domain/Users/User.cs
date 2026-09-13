using Vendora.BuildingBlocks.Results;

namespace Vendora.Services.Identity.Domain.Users;

public class User
{
    public Guid Id { get; init; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? EmailVerifiedAt { get; private set; }
    public bool IsEmailVerified => EmailVerifiedAt is not null;

    private User()
    {
    }

    public static Result<User> CreateCustomer(
        string email,
        string passwordHash,
        string fullName,
        string phoneNumber,
        DateTime createdAt)
    {
        email = NormalizeEmail(email);

        if (string.IsNullOrEmpty(email))
        {
            return Result<User>.Failure(new Error
            {
                Code = "user_create_customer.email_empty",
                Message = "Email must be not null.",
                Type = ErrorType.Validation
            });
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new InvalidOperationException("Data invalid. Password Hash is NULL or EMPTY.");
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            return Result<User>.Failure(new Error
            {
                Code = "user_create_customer.full_name_empty",
                Message = "Full name is required.",
                Type = ErrorType.Validation
            });
        }

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return Result<User>.Failure(new Error
            {
                Code = "user_create_customer.phone_number_empty",
                Message = "Phone number is required.",
                Type = ErrorType.Validation
            });
        }

        return Result<User>.Success(new User
        {
            Id = Guid.NewGuid(),
            Email = NormalizeEmail(email),
            PasswordHash = passwordHash,
            FullName = fullName,
            PhoneNumber = phoneNumber,
            Role = UserRole.Customer,
            Status = UserStatus.Active,
            CreatedAt = createdAt,
            UpdatedAt = null,
            EmailVerifiedAt = null
        });
    }

    private static string NormalizeEmail(string email)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return normalizedEmail;
    }

    public void Rename(string fullName, DateTime updatedAt)
    {
        FullName = fullName;
        UpdatedAt = updatedAt;
    }

    public void ChangePhoneNumber(string phoneNumber, DateTime updatedAt)
    {
        PhoneNumber = phoneNumber;
        UpdatedAt = updatedAt;
    }

    public void ChangePasswordHash(string passwordHash, DateTime updatedAt)
    {
        PasswordHash = passwordHash;
        UpdatedAt = updatedAt;
    }

    public void VerifyEmail(DateTime verifiedAt)
    {
        if (EmailVerifiedAt.HasValue)
            return;
        
        EmailVerifiedAt = verifiedAt;
        UpdatedAt = verifiedAt;
    }

    public void Delete(DateTime updatedAt)
    {
        if (Status is UserStatus.Deleted)
            return;
        
        Status = UserStatus.Deleted;
        UpdatedAt = updatedAt;
    }

    public Result Suspend(DateTime updatedAt)
    {
        switch (Status)
        {
            case UserStatus.Deleted:
                return Result.Failure(new Error
                {
                    Code = "user_suspend.deleted",
                    Message = "Cannot suspend a deleted user.",
                    Type = ErrorType.Conflict
                });
            
            case UserStatus.Suspended:
                return Result.Success();
            
            case UserStatus.Active:
                Status = UserStatus.Suspended;
                UpdatedAt = updatedAt;
                return Result.Success();
            
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(Status),
                    Status,
                    "Unsupported user status.");
        }
    }

    public Result Restore(DateTime updatedAt)
    {
        switch (Status)
        {
            case UserStatus.Suspended:
                return Result.Failure(new Error
                {
                    Code = "user_restore.suspended",
                    Message = "Cannot restore a suspended user."
                });
            
            case UserStatus.Active:
                return Result.Success();
            
            case UserStatus.Deleted:
                Status = UserStatus.Active;
                UpdatedAt = updatedAt;
                return Result.Success();
            
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(Status),
                    Status,
                    "Unsupported user status.");
        }
    }

    public Result Reactivate(DateTime updatedAt)
    {
        switch (Status)
        {
            case UserStatus.Deleted:
                return Result.Failure(new Error
                {
                    Code = "user_reactivate.deleted",
                    Message = "Cannot reactivate a deleted user.",
                    Type = ErrorType.Conflict
                });
            
            case UserStatus.Active:
                return Result.Success();
            
            case UserStatus.Suspended:
                Status = UserStatus.Active;
                UpdatedAt = updatedAt;
                return Result.Success();
            
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(Status),
                    Status,
                    "Unsupported user status.");
        }
    }
}