namespace Vendora.Services.Identity.Infrastructure.Redis;

public static class RedisKeys
{
    private const string Prefix = "identity";

    public static string EmailVerification(Guid userId)
        => $"{Prefix}:email-verification:{userId}";
}