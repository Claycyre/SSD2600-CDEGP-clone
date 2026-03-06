using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SSD2600_CDEGP.Models
{
    [Table("OrderLineItem")]
    [PrimaryKey(nameof(FkOrderId), nameof(FkProductSKU))]
    public class OrderLineItem
    {
        [Required]
        public int FkOrderId { get; set; }

        [Required]
        public int FkProductSKU { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double UnitPrice { get; set; }

        [ForeignKey(nameof(FkOrderId))]
        public Order? Order { get; set; }

        [ForeignKey(nameof(FkProductSKU))]
        public Product? Product { get; set; }
    }
}
