using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Vendora.Services.Identity.Application.Abstractions.Email;
using Vendora.Services.Identity.Infrastructure.Options;

namespace Vendora.Services.Identity.Infrastructure.Email;

public class SmtpEmailSender(IOptions<SmtpOptions> options) : IEmailSender
{
    private readonly SmtpOptions _smtpOptions = options.Value;
    public async Task SendAsync(string recipient, string subject, string body, CancellationToken cancellationToken)
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
        message.Body = body;

        await smtpClient.SendMailAsync(message, cancellationToken);
    }
}