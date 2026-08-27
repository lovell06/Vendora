namespace Vendora.Services.Identity.Application.Abstractions.Email;

public interface IEmailSender
{
    public Task SendAsync(
        string recepient, 
        string subject, 
        string body, 
        CancellationToken cancellationToken);
}