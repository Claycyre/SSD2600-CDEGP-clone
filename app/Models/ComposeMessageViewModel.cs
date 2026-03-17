using System.ComponentModel.DataAnnotations;

namespace SSD2600_CDEGP.Models;

public class ComposeMessageViewModel
{
    [Required]
    public string RecipientId { get; set; } = string.Empty;

    public string RecipientName { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;
}
