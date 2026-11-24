using System.Net.Mime;

namespace Vendora.Services.Identity.Infrastructure.Email;

public class SmtpEmailSender(IOptions<SmtpOptions> options) : IEmailSender
{
    private readonly SmtpOptions _smtpOptions = options.Value;
    public async Task SendAsync(
        string recipient,
        string subject,
        string body,
        CancellationToken cancellationToken,
        string? htmlBody = null)
    {
        using var smtpClient = new SmtpClient();
        smtpClient.Host = _smtpOptions.Host;
        smtpClient.Port = _smtpOptions.Port;
        smtpClient.Credentials = new NetworkCredential(
            userName: _smtpOptions.UserName, 
            password: _smtpOptions.Password);
        smtpClient.EnableSsl = true;

        using var message = new MailMessage();
        message.From = new MailAddress(
            address: _smtpOptions.UserName,
            displayName: _smtpOptions.DisplayName);
        message.To.Add(new MailAddress(recipient));
        message.Subject = subject;
        message.SubjectEncoding = Encoding.UTF8;
        message.Body = body;
        message.BodyEncoding = Encoding.UTF8;

        // Plain text stays as the fallback for clients that do not render HTML.
        if (htmlBody is not null)
        {
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
                htmlBody,
                Encoding.UTF8,
                MediaTypeNames.Text.Html));
        }

        await smtpClient.SendMailAsync(message, cancellationToken);
    }
}