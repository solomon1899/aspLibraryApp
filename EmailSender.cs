using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Macs;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string body, string filePath)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings");

        using (var client = new SmtpClient("smtp-relay.brevo.com")
        {
            //Port = int.Parse(smtpSettings["Port"]),
            Port = 587,
            Credentials = new NetworkCredential("elkhorchisoulaimane@gmail.com", "YOfwjTFhyG7gJ9AN"),
            EnableSsl = true,
        })
        {
            var message = new MailMessage
            {
                Subject = subject,
                Body = body,
                //IsBodyHtml = true,
            };

            // Set the From and To email addresses
            message.From = new MailAddress("brucewayne@darkknight.com");
            message.To.Add(new MailAddress(recipientEmail));

            var attachment = new Attachment(filePath);
            message.Attachments.Add(attachment);

            await client.SendMailAsync(message);
        }
    }
}
