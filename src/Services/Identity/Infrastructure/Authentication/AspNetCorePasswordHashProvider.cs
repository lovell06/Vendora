using Microsoft.AspNetCore.Identity;
using Vendora.Services.Identity.Application.Abstractions.Authentication;

namespace Vendora.Services.Identity.Infrastructure.Authentication;

public class AspNetCorePasswordHashProvider : IPasswordHashProvider
{
    private sealed class PasswordHashContext;

    private static readonly PasswordHashContext Context = new();

    private readonly PasswordHasher<PasswordHashContext> _passwordHasher = new();

    public string Hash(string passwordRaw)
    {
        return _passwordHasher.HashPassword(
            user: Context, 
            password: passwordRaw);
    }

    public bool Verify(string passwordHash, string passwordRaw)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            user: Context,
            hashedPassword: passwordHash,
            providedPassword: passwordRaw);

        return result is PasswordVerificationResult.Success
            or PasswordVerificationResult.SuccessRehashNeeded;
    }
}