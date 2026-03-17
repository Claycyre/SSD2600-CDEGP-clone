using System.ComponentModel.DataAnnotations;

namespace SSD2600_CDEGP.Models;

public enum UserRole
{
    [Display(Name = "Ordinary User")]
    PrivateCitizen,

    [Display(Name = "Purchase Manager")]
    PurchaseManager,

    [Display(Name = "Site Administrator")]
    SiteAdmin,

    [Display(Name = "Supplier")]
    Supplier,
}
