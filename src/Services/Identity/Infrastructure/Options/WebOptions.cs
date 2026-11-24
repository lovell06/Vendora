namespace Vendora.Services.Identity.Infrastructure.Options;

// Public address of Vendora Web. Emails link to pages on it (e.g. /verify-email).
public sealed class WebOptions
{
    public const string SectionName = "Web";

    public required string BaseUrl { get; init; }
}
