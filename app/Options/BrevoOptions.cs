using System.ComponentModel.DataAnnotations;

namespace SSD2600_CDEGP.Options;

public class BrevoOptions
{
    public const string SectionName = "Brevo";

    // Fail early (on start) if API key missing
    // Disregard squiggles - we don't want to nullify these
    // Program.cs - .ValidateOnStart() will crash the program
    [Required(ErrorMessage = "Brevo ApiKey is missing! Check your .env file.")]
    public string ApiKey { get; set; } = default!;

    [Required]
    public string SenderEmail { get; set; } = default!;

    [Required]
    public string SenderName { get; set; } = default!;
}
