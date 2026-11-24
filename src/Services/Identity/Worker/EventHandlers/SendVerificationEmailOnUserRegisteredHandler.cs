using Microsoft.Extensions.Options;
using Vendora.Services.Identity.Application.Abstractions.Email;
using Vendora.Services.Identity.Worker.Emails;

namespace Vendora.Services.Identity.Worker.EventHandlers;

public sealed class SendVerificationEmailOnUserRegisteredHandler(
    IEmailVerificationTokenProvider emailVerificationTokenProvider,
    IEmailSender sender,
    IOptions<WebOptions> webOptions) : IIntegrationEventHandler<UserRegisteredEvent>
{
    public async Task HandleAsync(UserRegisteredEvent @event, CancellationToken ct)
    {
        var verificationToken = await emailVerificationTokenProvider.IssueAsync(
            userId: @event.UserId,
            cancellationToken: ct);

        var verifyUrl = VerificationEmail.BuildVerifyUrl(
            webBaseUrl: webOptions.Value.BaseUrl,
            userId: @event.UserId,
            token: verificationToken);

        var ttl = EmailConstants.EmailVerificationTokenTtl;

        await sender.SendAsync(
            recepient: @event.UserEmail,
            subject: VerificationEmail.Subject,
            body: VerificationEmail.PlainText(@event.UserEmail, verifyUrl, ttl),
            cancellationToken: ct,
            htmlBody: VerificationEmail.Html(@event.UserEmail, verifyUrl, ttl));
    }
}
