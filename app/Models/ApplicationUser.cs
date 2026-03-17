using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SSD2600_CDEGP.Models;

public class ApplicationUser : IdentityUser
{
    public int? FkSupplierId { get; set; }

    [ForeignKey(nameof(FkSupplierId))]
    public Supplier? Supplier { get; set; }

    public int? FkContactId { get; set; }

    [ForeignKey(nameof(FkContactId))]
    public ContactDetail? ContactDetail { get; set; }

    /// <summary>ISO 4217 currency code the user prefers, e.g. CAD, USD, EUR. Defaults to CAD.</summary>
    [StringLength(3)]
    public string PreferredCurrencyCode { get; set; } = "CAD";

    /// <summary>The type of account: SiteAdmin, Supplier, PurchaseManager, or PrivateCitizen.</summary>
    [StringLength(50)]
    public string UserRole { get; set; } = "PrivateCitizen";

    /// <summary>The date and time when the account was registered.</summary>
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    /// <summary>Whether the user has submitted identity verification (for org accounts).</summary>
    public bool VerificationSubmitted { get; set; } = false;

    /// <summary>Whether the admin has approved the user's identity verification.</summary>
    public bool VerificationApproved { get; set; } = false;

    ///// <summary>Relative path to the uploaded verification document under wwwroot (e.g. /uploads/verification/userId/file.pdf).</summary>
    //[StringLength(500)]
    //public string? VerificationDocumentPath { get; set; }

    /// <summary>Whether this account has been permanently banned by a site admin.</summary>
    public bool UserBanned { get; set; } = false;

    /// <summary>Whether this account has been temporarily suspended by a site admin.</summary>
    public bool UserSuspended { get; set; } = false;
}
