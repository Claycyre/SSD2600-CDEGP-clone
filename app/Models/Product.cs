using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSD2600_CDEGP.Models;

[Table("Product")]
public class Product
{
    [Key]
    public int PkSKU { get; set; }

    [Required]
    [StringLength(70)]
    public string Name { get; set; } = string.Empty;

    [StringLength(30)]
    public string? ShortName { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    public byte[]? Media { get; set; }

    [Required]
    public double UnitPrice { get; set; }

    [Required]
    public int StockQuantity { get; set; }

    public byte[]? Attributes { get; set; }

    /// <summary>Atomic number of the primary isotope element (links to the periodic table).</summary>
    public int? AtomicNumber { get; set; }

    /// <summary>Physical state of the product at room temperature, e.g. Solid, Liquid, Gas.</summary>
    [Required]
    [StringLength(50)]
    public string StateOfMatter { get; set; } = string.Empty;

    /// <summary>High-level product category, e.g. Medical, Industrial, Research.</summary>
    [Required]
    [StringLength(50)]
    public string ProductType { get; set; } = string.Empty;

    /// <summary>Subcategory within the product type, e.g. Diagnostic, Therapeutic, NDT.</summary>
    [StringLength(60)]
    public string? ProductSubtype { get; set; }

    /// <summary>Radioactive half-life of the isotope, e.g. "6 h", "8.02 days".</summary>
    [StringLength(30)]
    public string? HalfLife { get; set; }

    /// <summary>Radionuclidic or radiochemical purity specification, e.g. "≥99.9 %".</summary>
    [StringLength(60)]
    public string? Purity { get; set; }

    /// <summary>Specific activity at calibration time, e.g. "≥185 GBq/mg".</summary>
    [StringLength(80)]
    public string? SpecificActivity { get; set; }

    /// <summary>True when a site admin has approved this product for public display.</summary>
    public bool IsAdminVerified { get; set; } = false;

    [Required]
    [ForeignKey("Supplier")]
    public int? FkSupplierId { get; set; }

    // Navigation property
    public Supplier? Supplier { get; set; }
}
