using brevo_csharp.Model;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Services.Interfaces;

public interface IEmailService
{
    Task<CreateSmtpEmail> SendEmailAsync(EmailModel payload);
}
