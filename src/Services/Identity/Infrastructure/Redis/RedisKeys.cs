namespace Vendora.Services.Identity.Infrastructure.Redis;

public static class RedisKeys
{
    private const string Prefix = "identity";

    public static string EmailVerification(Guid userId)
        => $"{Prefix}:email-verification:{userId}";

    public static string RefreshToken(Guid userId, Guid refreshTokenId)
        => $"{Prefix}:refresh-token:{userId}:{refreshTokenId}";

    public static string AccessTokenBlackList(Guid tokenId) 
        => $"{Prefix}:access-token:black-list:{tokenId}";
}