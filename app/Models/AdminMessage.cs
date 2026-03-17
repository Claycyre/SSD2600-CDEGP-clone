using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSD2600_CDEGP.Models;

public class AdminMessage
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FkSenderId { get; set; } = string.Empty;

    [ForeignKey(nameof(FkSenderId))]
    public ApplicationUser Sender { get; set; } = null!;

    [Required]
    public string FkRecipientId { get; set; } = string.Empty;

    [ForeignKey(nameof(FkRecipientId))]
    public ApplicationUser Recipient { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public bool IsRead { get; set; } = false;
}
