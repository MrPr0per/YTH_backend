using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;

namespace YTH_backend.Infrastructure.Email;

public class AwsSesEmailService : IEmailService
{
    private readonly IAmazonSimpleEmailServiceV2 client;
    private readonly string fromEmail;

    public AwsSesEmailService(string smtpHost, string ycAccessKey, string ycSecretKey, string fromEmail)
    {
        var credentials = new BasicAWSCredentials(ycAccessKey, ycSecretKey);
        var config = new AmazonSimpleEmailServiceV2Config
        {
            ServiceURL = smtpHost,
            AuthenticationRegion = "ru-central1"
        };
        
        this.fromEmail = fromEmail;
        client = new AmazonSimpleEmailServiceV2Client(credentials, config);
    }
    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var html = new Content();
        var subjectContent = new Content();
        html.Data = htmlBody;
        subjectContent.Data = subject;
        
        var request = new SendEmailRequest
        {
            FromEmailAddress = $"{fromEmail}",
            Destination = new Destination { ToAddresses = new List<string> { to } },
            Content = new EmailContent
            {
                Simple = new Message
                {
                    Subject = subjectContent,
                    Body = new Body { Html = html }
                }
            }
        };
        
        await client.SendEmailAsync(request);
    }
}