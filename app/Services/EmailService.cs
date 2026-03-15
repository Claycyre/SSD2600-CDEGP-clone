using brevo_csharp.Api;
using brevo_csharp.Model;
using Microsoft.Extensions.Options;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Options;
using SSD2600_CDEGP.Services.Interfaces;

namespace SSD2600_CDEGP.Services;

public class EmailService(IOptions<BrevoOptions> options) : IEmailService
{
    private readonly BrevoOptions _settings = options.Value;

    // Create the engine once when the service is born
    private readonly TransactionalEmailsApi _apiInstance = new();

    public async Task<CreateSmtpEmail> SendEmailAsync(EmailModel payload)
    {
        var senderEmail = _settings.SenderEmail;
        var senderName = _settings.SenderName;

        var email = new SendSmtpEmail(
            sender: new SendSmtpEmailSender(senderName, senderEmail),
            to: [new SendSmtpEmailTo(payload.RecipientEmail)],
            subject: payload.Subject,
            htmlContent: payload.Body
        );

        // Returns the Brevo response object containing the MessageId
        return await _apiInstance.SendTransacEmailAsync(email);
    }
}
