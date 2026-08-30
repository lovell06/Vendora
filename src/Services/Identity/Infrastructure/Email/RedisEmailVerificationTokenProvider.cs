using System.Security.Cryptography;
using System.Text;
using StackExchange.Redis;
using Vendora.Services.Identity.Application.Abstractions.Email;
using Vendora.Services.Identity.Infrastructure.Redis;

namespace Vendora.Services.Identity.Infrastructure.Email;

public sealed class RedisEmailVerificationTokenProvider(IConnectionMultiplexer connection) : IEmailVerificationTokenProvider
{
    private const string TokenChoices = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private readonly IDatabase _database = connection.GetDatabase();
    public async Task<string> IssueAsync(Guid userId, CancellationToken cancellationToken)
    {
        var token = RandomNumberGenerator.GetString(TokenChoices, 64);

        var tokenHash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        var key = RedisKeys.EmailVerification(userId);
        var ttl = TimeSpan.FromMinutes(5);

        await _database.StringSetAsync(
            key: key,
            value: tokenHash,
            expiry: ttl);

        return token;
    }

    public async Task RevokeAsync(Guid userId, CancellationToken cancellationToken)
    {
        var key = RedisKeys.EmailVerification(userId);

        await _database.KeyDeleteAsync(key);
    }

    public async Task<bool> ValidateAsync(
        Guid userId,
        string token,
        CancellationToken cancellationToken)
    {
        var key = RedisKeys.EmailVerification(userId);

        var value = await _database.StringGetAsync(key);

        if (!value.HasValue)
            return false;

        var expectedToken = Convert.FromBase64String(value!);
        var actualToken = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return CryptographicOperations.FixedTimeEquals(actualToken, expectedToken);
    }
}