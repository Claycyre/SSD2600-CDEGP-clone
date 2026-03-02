using System.ComponentModel.DataAnnotations;

namespace SSD2600_CDEGP.Models;

public class Supplier
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [MinLength(3)]
    [StringLength(20)]
    public string? ShortName { get; set; }

    [Required]
    [MinLength(3)]
    [StringLength(3)]
    public string CurrencyCode { get; set; } = "CAD";

    public ICollection<ApplicationUser> Users { get; } = [];
}
