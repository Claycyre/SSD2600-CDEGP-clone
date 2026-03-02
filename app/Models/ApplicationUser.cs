using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SSD2600_CDEGP.Models;

public class ApplicationUser : IdentityUser
{
    public int? FkSupplierId { get; set; }

    [ForeignKey(nameof(FkSupplierId))]
    public Supplier? Supplier { get; set; }
}
