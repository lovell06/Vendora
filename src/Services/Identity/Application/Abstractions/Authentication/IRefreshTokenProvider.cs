namespace Vendora.Services.Identity.Application.Abstractions.Authentication;

public interface IRefreshTokenProvider
{
    Task<RefreshToken> IssueAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> ValidateAsync(Guid userId, RefreshToken token, CancellationToken cancellationToken);
    Task RevokeAsync(Guid userId, Guid tokenId, CancellationToken cancellationToken);
}

public sealed record RefreshToken(Guid Id, string Value);