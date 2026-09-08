namespace Vendora.Services.Identity.Application.Abstractions.Authentication;

public interface IRefreshTokenProvider
{
    Task<string> IssueAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> ValidateAsync(Guid userId, string token, CancellationToken cancellationToken);
    Task RevokeAsync(Guid userId, CancellationToken cancellationToken);
}