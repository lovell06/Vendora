namespace Vendora.Services.Identity.Application.Abstractions.Authentication;

public interface IAccessTokenProvider
{
    string Issue(User user);
    Task RevokeAsync(Guid tokenId, CancellationToken ct);
}