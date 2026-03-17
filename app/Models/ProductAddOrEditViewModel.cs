using System.ComponentModel.DataAnnotations;

namespace SSD2600_CDEGP.Models;

public class ProductAddOrEditViewModel
{
    [Required]
    [StringLength(70)]
    public string Name { get; set; } = string.Empty;

    [StringLength(30)]
    public string? ShortName { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public double UnitPrice { get; set; }

    [Required]
    public int StockQuantity { get; set; }

    /// <summary>Atomic number of the primary isotope element (links to the periodic table).</summary>
    public int? AtomicNumber { get; set; }

    [Required]
    [StringLength(50)]
    public string StateOfMatter { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string ProductType { get; set; } = string.Empty;

    [StringLength(60)]
    public string? ProductSubtype { get; set; }

    [StringLength(30)]
    public string? HalfLife { get; set; }

    [StringLength(60)]
    public string? Purity { get; set; }

    [StringLength(80)]
    public string? SpecificActivity { get; set; }

    public string? ImageUrl { get; set; }
}
