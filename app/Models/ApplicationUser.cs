using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SSD2600_CDEGP.Models;

public class ApplicationUser : IdentityUser
{
    public int? FkSupplierId { get; set; }

    [ForeignKey(nameof(FkSupplierId))]
    public Supplier? Supplier { get; set; }

    /// <summary>ISO 4217 currency code the user prefers, e.g. CAD, USD, EUR. Defaults to CAD.</summary>
    [StringLength(3)]
    public string PreferredCurrencyCode { get; set; } = "CAD";
}
