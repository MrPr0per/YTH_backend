using MailKit.Net.Smtp;
using MimeKit;

namespace YTH_backend.Infrastructure.Email;

public class MailKitEmailService(string smtpHost, int smtpPort, string smtpUser, string smtpPassword, string fromEmail, string fromName) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(smtpUser, smtpPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}