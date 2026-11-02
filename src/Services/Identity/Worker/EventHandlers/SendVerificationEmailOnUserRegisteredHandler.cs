using Vendora.Services.Identity.Application.Abstractions.Email;

namespace Vendora.Services.Identity.Worker.EventHandlers;

public sealed class SendVerificationEmailOnUserRegisteredHandler(
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