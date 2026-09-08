using System.Security.Cryptography;
using System.Text;
using StackExchange.Redis;
using Vendora.Services.Identity.Application.Abstractions.Authentication;
using Vendora.Services.Identity.Infrastructure.Redis;

namespace Vendora.Services.Identity.Infrastructure.Authentication;

public class RandomNumberTokenProvider(IConnectionMultiplexer multiplexer) : IRefreshTokenProvider
{
    private readonly IDatabase _database = multiplexer.GetDatabase();

    public async Task<string> IssueAsync(Guid userId, CancellationToken cancellationToken)
    {
        var token = RandomNumberGenerator.GetHexString(32, true);

        var tokenHash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        await _database.StringSetAsync(
            key: RedisKeys.RefreshToken(userId),
            value: tokenHash,
            expiry: AuthenticationConstants.RefreshTokenTtl);

        return token;
    }

    public async Task RevokeAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _database.KeyDeleteAsync(RedisKeys.RefreshToken(userId));
    }

    public async Task<bool> ValidateAsync(Guid userId, string token, CancellationToken cancellationToken)
    {
        var value = await _database.StringGetAsync(RedisKeys.RefreshToken(userId));

        if (!value.HasValue)
            return false;

        var expectedTokenHash = Convert.FromBase64String(value!);
        var actualTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return CryptographicOperations.FixedTimeEquals(actualTokenHash, expectedTokenHash);
    }
}