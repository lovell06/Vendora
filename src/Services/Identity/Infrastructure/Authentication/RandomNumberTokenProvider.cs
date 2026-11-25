namespace Vendora.Services.Identity.Infrastructure.Authentication;

public sealed class RandomNumberTokenProvider(IConnectionMultiplexer multiplexer) : IRefreshTokenProvider
{
    private readonly IDatabase _database = multiplexer.GetDatabase();

    public async Task<RefreshToken> IssueAsync(Guid userId, CancellationToken cancellationToken)
    {
        var tokenId = Guid.CreateVersion7();
        var token = RandomNumberGenerator.GetHexString(32, true);

        var tokenHash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        await _database.StringSetAsync(
            key: RedisKeys.RefreshToken(userId, tokenId),
            value: tokenHash,
            expiry: AuthenticationConstants.RefreshTokenTtl);

        return new RefreshToken(tokenId, token);
    }

    public async Task RevokeAsync(Guid userId, Guid tokenId, CancellationToken cancellationToken)
    {
        await _database.KeyDeleteAsync(RedisKeys.RefreshToken(userId, tokenId));
    }

    public async Task<bool> ValidateAsync(Guid userId, RefreshToken token, CancellationToken cancellationToken)
    {
        var value = await _database.StringGetAsync(RedisKeys.RefreshToken(userId, token.Id));

        if (!value.HasValue)
            return false;

        var expectedTokenHash = Convert.FromBase64String(value!);
        var actualTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(token.Value));

        return CryptographicOperations.FixedTimeEquals(actualTokenHash, expectedTokenHash);
    }
}