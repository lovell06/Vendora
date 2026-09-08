namespace Vendora.Services.Identity.Infrastructure.Email;

public static class EmailConstants
{
    public static readonly TimeSpan EmailVerificationTokenTtl = TimeSpan.FromMinutes(5);

    public const string EmailVerificationTokenChoices =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
}