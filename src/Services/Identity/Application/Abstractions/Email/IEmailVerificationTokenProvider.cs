namespace Vendora.Services.Identity.Application.Abstractions.Email;

public interface IEmailVerificationTokenProvider
{
    public Task<string> IssueAsync(
        Guid userId,
        CancellationToken cancellationToken);

    public Task<bool> ValidateAsync(
        Guid userId,
        string token,
        CancellationToken cancellationToken);

    public Task RevokeAsync(
        Guid userId,
        CancellationToken cancellationToken);
}