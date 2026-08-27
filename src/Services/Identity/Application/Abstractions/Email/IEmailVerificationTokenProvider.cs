namespace Vendora.Services.Identity.Application.Abstractions.Email;

public interface IEmailVerificationTokenProvider
{
    public Task<string> IssueAsync(
        Guid userId,
        string email,
        CancellationToken cancellationToken);

    public Task<bool> ValidateAsync(
        Guid userId,
        string email,
        string token,
        CancellationToken cancellationToken);

    public Task RevokeAsync(
        Guid userId,
        CancellationToken cancellationToken);
}