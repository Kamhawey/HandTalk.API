using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Module.Identity.Persistence.Email;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Shared.Infrastructure.Email;
public class EmailService(ILogger<EmailService> logger, IOptions<MailSettings> emailSettings)
    : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;
    private readonly MailSettings _emailSettings = emailSettings.Value;

    public async Task SendAsync(string to, string subject, string token)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.UserName));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html)
        {
            Text =  token
        };

        //send email message
        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
        await smtpClient.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
        await smtpClient.SendAsync(message);
    }
}