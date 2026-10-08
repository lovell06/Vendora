namespace Vendora.Services.Identity.Application.Authentication.SendVerificationEmail;

public sealed class Handler(
    IEmailVerificationTokenProvider emailVerificationTokenProvider,
    IEmailSender sender) : IIntegrationEventHandler<UserRegisteredEvent>
{
    public async Task HandleAsync(UserRegisteredEvent @event, CancellationToken ct)
    {
        var verificationToken = await emailVerificationTokenProvider.IssueAsync(
            userId: @event.UserId,
            cancellationToken: ct);
        
        await sender.SendAsync(
            recepient: @event.UserEmail,
            subject: "Vendora Account Verification",
            body: $"UserID=\'{@event.UserId}\'\nToken=\'{verificationToken}\'",
            cancellationToken: ct);
    }
}