using MailKit.Net.Smtp;
using MimeKit;

namespace YTH_backend.Infrastructure.Email;

public class MailKitEmailService(string smtpHost, int smtpPort, string ycAccessKey, string ycSecretKey, string fromEmail, string fromName) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Headers.Add("X-YC-Mail-From", fromEmail);
        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect);
        
        
        await client.AuthenticateAsync(ycAccessKey, ycSecretKey);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}