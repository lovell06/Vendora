namespace Vendora.Services.Identity.Infrastructure.Options;

public sealed class SmtpOptions
{
    public const string SectionName = "Smtp";

    public required string DisplayName { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string Host { get; init; }
    public int Port { get; init; }
}