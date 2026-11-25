namespace Vendora.Services.Identity.Application.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }

    Task<string> GetAccessTokenAsync();
    Guid GetAccessTokenId();
    DateTimeOffset GetExpirationDateTime();
}