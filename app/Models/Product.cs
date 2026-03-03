using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSD2600_CDEGP.Models
{
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

        public string? Description { get; set; }

        public byte[]? Media { get; set; }

        [Required]
        public double UnitPrice { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public byte[]? Attributes { get; set; }

        [Required]
        [StringLength(10)]
        [ForeignKey("Supplier")]
        public string FkSupplierId { get; set; } = string.Empty;
    }
}
